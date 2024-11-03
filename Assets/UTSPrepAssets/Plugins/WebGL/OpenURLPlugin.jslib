mergeInto(LibraryManager.library, {
    OpenURLInNewTab: function(url) {
        var urlStr = UTF8ToString(url);
        var newWindow = window.open(urlStr, '_blank');
        if (newWindow) {
            newWindow.focus();
        }
    }
});