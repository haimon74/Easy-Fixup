﻿define('router',
    ['jquery', 'underscore', 'sammy', 'presenter', 'config', 'route-mediator', 'store'],
    function ($, _, Sammy, presenter, config, routeMediator, store) {
        var
            currentHash = '',
            defaultRoute = '',
            isRedirecting = false,
            logger = config.logger,
            startupUrl = '',
            window = config.window,

            sammy = new Sammy.Application(function () {
                if (Sammy.Title) {
                    this.use(Sammy.Title);
                    this.setTitle(config.title);
                }
            }),

            navigateBack = function () {
                window.history.back();
            },

            navigateTo = function (url) {
                sammy.setLocation(url);
            },

            register = function (options) {
                if (options.routes) {
                    // Register a list of routes
                    _.each(options.routes, function (route) {
                        registerRoute({
                            route: route.route,
                            title: route.title,
                            callback: route.callback,
                            view: options.view,
                            group: route.group,
                            isDefault: !!route.isDefault
                        });
                    });
                    return;
                }

                // Register 1 route
                registerRoute(options);
            },

            registerBeforeLeaving = function () {
                sammy.before(/.*/, function () {
                    var
                        context = this,
                        response = routeMediator.canLeave();

                    if (!isRedirecting && !response.val) {
                        isRedirecting = true;
                        logger.warning(response.message);
                        // Keep hash url the same in address bar
                        context.app.setLocation(currentHash);
                    }
                    else {
                        isRedirecting = false;
                        currentHash = context.app.getLocation();
                    }
                    // Cancel the route if this returns false
                    return response.val;
                });
            },

            registerRoute = function (options) {
                if (!options.callback) {
                    throw Error('callback must be specified.');
                }

                if (options.isDefault) {
                    defaultRoute = options.route;
                    setupGet(options, '/');
                }

                setupGet(options);
            },
            
            setupGet = function (options, routeOverride) {
                var route = routeOverride || options.route;
                sammy.get(route, function (context) { //context is 'this'
                    store.save(config.stateKeys.lastView, context.path);
                    options.callback(context.params); // Activate the viewmodel
                    $('.view').hide();
                    presenter.transitionTo(
                        $(options.view),
                        options.route, //context.path, // We want to find the route we defined in the config
                        options.group
                    );
                    if (this.title) {
                        this.title(options.title);
                    }
                });
            },
            
            getUsableRoute = function (value) {
                return value && value.indexOf('/#') != -1 ? value : null;
            },

            run = function () {
                var url = store.fetch(config.stateKeys.lastView);

                // 1) if i browse to a location, use it
                // 2) otherwise use the url i grabbed from storage
                // 3) otherwise use the default route
                var addressBarUrl = sammy.getLocation();
                startupUrl = getUsableRoute(addressBarUrl) || getUsableRoute(url) || defaultRoute;
                
                sammy.run();
                registerBeforeLeaving();
                navigateTo(startupUrl);
           };

        return {
            navigateBack: navigateBack,
            navigateTo: navigateTo,
            register: register,
            run: run
        };
    });