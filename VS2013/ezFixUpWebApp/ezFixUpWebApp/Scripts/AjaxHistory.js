Type.registerNamespace('ezFixUp.Components');
ezFixUp.Components.$create_AjaxHistoryOptions = function ezFixUp_AjaxHistoryOptions() { return {}; }

ezFixUp.Components.AjaxHistory = function ezFixUp_AjaxHistory() {
    ezFixUp.Components.AjaxHistory.initializeBase(this);
}
ezFixUp.Components.AjaxHistory.prototype = {
    _timerCookie$1: 0,
    _timerHandler$1: null,
    _iframeLoadHandler$1: null,
    _appLoadHandler$1: null,
    _endRequestHandler$1: null,
    _ignoreIFrame$1: false,
    _ignoreTimer$1: false,
    _historyFrame$1: null,
    _emptyPageURL$1: null,
    _dataID$1: null,
    _postbackID$1: null,
    _currentEntry$1: null,
    
    get_options: function ezFixUp_AjaxHistory$get_options() {
        return null;
    },
    set_options: function ezFixUp_AjaxHistory$set_options(value) {
        this._currentEntry$1 = (value.initialEntry) ? value.initialEntry : '';
        this._dataID$1 = value.dataID;
        this._postbackID$1 = value.postbackID;
        return value;
    },
    
    _addEntry$1: function ezFixUp_AjaxHistory$_addEntry$1(entry) {
        this._ignoreTimer$1 = true;
        if (this._historyFrame$1) {
            this._ignoreIFrame$1 = true;
            this._historyFrame$1.src = this._emptyPageURL$1 + entry;
        }
        else {
            this._setEntry$1(entry);
        }
    },
    
    dispose: function ezFixUp_AjaxHistory$dispose() {
        if (this._historyFrame$1) {
            this._historyFrame$1.detachEvent('onload', this._iframeLoadHandler$1);
            this._historyFrame$1 = null;
        }
        if (this._timerCookie$1) {
            window.clearTimeout(this._timerCookie$1);
            this._timerCookie$1 = 0;
        }
        if (this._endRequestHandler$1) {
            Sys.WebForms.PageRequestManager.getInstance().remove_endRequest(this._endRequestHandler$1);
            this._endRequestHandler$1 = null;
        }
        ezFixUp.Components.AjaxHistory.callBaseMethod(this, 'dispose');
    },
    
    _getEntry$1: function ezFixUp_AjaxHistory$_getEntry$1() {
        var entry = window.location.hash;
        if ((entry.length >= 1) && (entry.charAt(0) === '#')) {
            entry = entry.substr(1);
        }
        return entry;
    },
    
    initialize: function ezFixUp_AjaxHistory$initialize() {
        ezFixUp.Components.AjaxHistory.callBaseMethod(this, 'initialize');
        this._appLoadHandler$1 = Function.createDelegate(this, this._onAppLoad$1);
        Sys.Application.add_load(this._appLoadHandler$1);
    },
    
    _navigate$1: function ezFixUp_AjaxHistory$_navigate$1(entry) {
        __doPostBack(this._postbackID$1, entry);
    },
    
    _onAppLoad$1: function ezFixUp_AjaxHistory$_onAppLoad$1(sender, e) {
        Sys.Application.remove_load(this._appLoadHandler$1);
        this._appLoadHandler$1 = null;
        this._endRequestHandler$1 = Function.createDelegate(this, this._onPageRequestManagerEndRequest$1);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(this._endRequestHandler$1);
        if (window.navigator.userAgent.indexOf('MSIE') >= 0) {
            this._historyFrame$1 = $get('__historyFrame');
            this._emptyPageURL$1 = this._historyFrame$1.src + '?';
            this._iframeLoadHandler$1 = Function.createDelegate(this, this._onIFrameLoad$1);
            this._historyFrame$1.attachEvent('onload', this._iframeLoadHandler$1);
        }
        this._timerHandler$1 = Function.createDelegate(this, this._onTick$1);
        this._timerCookie$1 = window.setTimeout(this._timerHandler$1, 100);
        var loadedEntry = this._getEntry$1();
        if (loadedEntry !== this._currentEntry$1) {
            this._currentEntry$1 = loadedEntry;
            this._navigate$1(loadedEntry);
        }
    },
    
    _onIFrameLoad$1: function ezFixUp_AjaxHistory$_onIFrameLoad$1() {
        var entry = this._historyFrame$1.contentWindow.location.search;
        if ((entry.length >= 1) && (entry.charAt(0) === '?')) {
            entry = entry.substr(1);
        }
        this._setEntry$1(entry);
        if (this._ignoreIFrame$1) {
            this._ignoreIFrame$1 = false;
            return;
        }
        this._navigate$1(entry);
    },
    
    _onPageRequestManagerEndRequest$1: function ezFixUp_AjaxHistory$_onPageRequestManagerEndRequest$1(sender, e) {
        var entry = e.get_dataItems()[this._dataID$1];
        if (entry) {
            this._addEntry$1(entry);
        }
    },
    
    _onTick$1: function ezFixUp_AjaxHistory$_onTick$1() {
        this._timerCookie$1 = 0;
        var entry = this._getEntry$1();
        if (entry !== this._currentEntry$1) {
            if (!this._ignoreTimer$1) {
                this._currentEntry$1 = entry;
                this._navigate$1(entry);
            }
        }
        else {
            this._ignoreTimer$1 = false;
        }
        this._timerCookie$1 = window.setTimeout(this._timerHandler$1, 100);
    },
    
    _setEntry$1: function ezFixUp_AjaxHistory$_setEntry$1(entry) {
        this._currentEntry$1 = entry;
        window.location.hash = entry;
    }
}

ezFixUp.Components.AjaxHistory.registerClass('ezFixUp.Components.AjaxHistory', Sys.Component);