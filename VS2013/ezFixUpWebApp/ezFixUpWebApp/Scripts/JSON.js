var JSON;
if (!JSON) {
    JSON = {};
}

(function(){"use strict";function f(n){return n<10?'0'+n:n;}
if(typeof Date.prototype.toJSON!=='function'){Date.prototype.toJSON=function(key){return isFinite(this.valueOf())?this.getUTCFullYear()+'-'+
f(this.getUTCMonth()+1)+'-'+
f(this.getUTCDate())+'T'+
f(this.getUTCHours())+':'+
f(this.getUTCMinutes())+':'+
f(this.getUTCSeconds())+'Z':null;};String.prototype.toJSON=Number.prototype.toJSON=Boolean.prototype.toJSON=function(key){return this.valueOf();};}
var cx=/[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,escapable=/[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,gap,indent,meta={'\b':'\\b','\t':'\\t','\n':'\\n','\f':'\\f','\r':'\\r','"':'\\"','\\':'\\\\'},rep;function quote(string){escapable.lastIndex=0;return escapable.test(string)?'"'+string.replace(escapable,function(a){var c=meta[a];return typeof c==='string'?c:'\\u'+('0000'+a.charCodeAt(0).toString(16)).slice(-4);})+'"':'"'+string+'"';}
function str(key,holder){var i,k,v,length,mind=gap,partial,value=holder[key];if(value&&typeof value==='object'&&typeof value.toJSON==='function'){value=value.toJSON(key);}
if(typeof rep==='function'){value=rep.call(holder,key,value);}
switch(typeof value){case'string':return quote(value);case'number':return isFinite(value)?String(value):'null';case'boolean':case'null':return String(value);case'object':if(!value){return'null';}
gap+=indent;partial=[];if(Object.prototype.toString.apply(value)==='[object Array]'){length=value.length;for(i=0;i<length;i+=1){partial[i]=str(i,value)||'null';}
v=partial.length===0?'[]':gap?'[\n'+gap+
partial.join(',\n'+gap)+'\n'+
mind+']':'['+partial.join(',')+']';gap=mind;return v;}
if(rep&&typeof rep==='object'){length=rep.length;for(i=0;i<length;i+=1){k=rep[i];if(typeof k==='string'){v=str(k,value);if(v){partial.push(quote(k)+(gap?': ':':')+v);}}}}else{for(k in value){if(Object.hasOwnProperty.call(value,k)){v=str(k,value);if(v){partial.push(quote(k)+(gap?': ':':')+v);}}}}
v=partial.length===0?'{}':gap?'{\n'+gap+partial.join(',\n'+gap)+'\n'+
mind+'}':'{'+partial.join(',')+'}';gap=mind;return v;}}
if(typeof JSON.stringify!=='function'){JSON.stringify=function(value,replacer,space){var i;gap='';indent='';if(typeof space==='number'){for(i=0;i<space;i+=1){indent+=' ';}}else if(typeof space==='string'){indent=space;}
rep=replacer;if(replacer&&typeof replacer!=='function'&&(typeof replacer!=='object'||typeof replacer.length!=='number')){throw new Error('JSON.stringify');}
return str('',{'':value});};}
if(typeof JSON.parse!=='function'){JSON.parse=function(){var state,stack,container,key,value,escapes={'\\':'\\','"':'"','/':'/','t':'\t','n':'\n','r':'\r','f':'\f','b':'\b'},string={go:function(){state='ok';},firstokey:function(){key=value;state='colon';},okey:function(){key=value;state='colon';},ovalue:function(){state='ocomma';},firstavalue:function(){state='acomma';},avalue:function(){state='acomma';}},number={go:function(){state='ok';},ovalue:function(){state='ocomma';},firstavalue:function(){state='acomma';},avalue:function(){state='acomma';}},action={'{':{go:function(){stack.push({state:'ok'});container={};state='firstokey';},ovalue:function(){stack.push({container:container,state:'ocomma',key:key});container={};state='firstokey';},firstavalue:function(){stack.push({container:container,state:'acomma'});container={};state='firstokey';},avalue:function(){stack.push({container:container,state:'acomma'});container={};state='firstokey';}},'}':{firstokey:function(){var pop=stack.pop();value=container;container=pop.container;key=pop.key;state=pop.state;},ocomma:function(){var pop=stack.pop();container[key]=value;value=container;container=pop.container;key=pop.key;state=pop.state;}},'[':{go:function(){stack.push({state:'ok'});container=[];state='firstavalue';},ovalue:function(){stack.push({container:container,state:'ocomma',key:key});container=[];state='firstavalue';},firstavalue:function(){stack.push({container:container,state:'acomma'});container=[];state='firstavalue';},avalue:function(){stack.push({container:container,state:'acomma'});container=[];state='firstavalue';}},']':{firstavalue:function(){var pop=stack.pop();value=container;container=pop.container;key=pop.key;state=pop.state;},acomma:function(){var pop=stack.pop();container.push(value);value=container;container=pop.container;key=pop.key;state=pop.state;}},':':{colon:function(){if(Object.hasOwnProperty.call(container,key)){throw new SyntaxError('Duplicate key "'+key+'"');}
state='ovalue';}},',':{ocomma:function(){container[key]=value;state='okey';},acomma:function(){container.push(value);state='avalue';}},'true':{go:function(){value=true;state='ok';},ovalue:function(){value=true;state='ocomma';},firstavalue:function(){value=true;state='acomma';},avalue:function(){value=true;state='acomma';}},'false':{go:function(){value=false;state='ok';},ovalue:function(){value=false;state='ocomma';},firstavalue:function(){value=false;state='acomma';},avalue:function(){value=false;state='acomma';}},'null':{go:function(){value=null;state='ok';},ovalue:function(){value=null;state='ocomma';},firstavalue:function(){value=null;state='acomma';},avalue:function(){value=null;state='acomma';}}};function debackslashify(text){return text.replace(/\\(?:u(.{4})|([^u]))/g,function(a,b,c){return b?String.fromCharCode(parseInt(b,16)):escapes[c];});}
return function(source,reviver){var r,tx=/^[\x20\t\n\r]*(?:([,:\[\]{}]|true|false|null)|(-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)|"((?:[^\r\n\t\\\"]|\\(?:["\\\/trnfb]|u[0-9a-fA-F]{4}))*)")/;state='go';stack=[];try{for(;;){r=tx.exec(source);if(!r){break;}
if(r[1]){action[r[1]][state]();}else if(r[2]){value=+r[2];number[state]();}else{value=debackslashify(r[3]);string[state]();}
source=source.slice(r[0].length);}}catch(e){state=e;}
if(state!=='ok'||/[^\x20\t\n\r]/.test(source)){throw state instanceof SyntaxError?state:new SyntaxError('JSON');}
return typeof reviver==='function'?(function walk(holder,key){var k,v,value=holder[key];if(value&&typeof value==='object'){for(k in value){if(Object.hasOwnProperty.call(value,k)){v=walk(value,k);if(v!==undefined){value[k]=v;}else{delete value[k];}}}}
return reviver.call(holder,key,value);}({'':value},'')):value;};};}}());