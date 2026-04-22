mergeInto(LibraryManager.library, {
    QuitGame: function() {
        if (typeof unityInstance !== 'undefined' && unityInstance !== null) {
            unityInstance.Quit().then(function() {
                console.log("Unity unloaded successfully");
                unityInstance = null;
            });
        }
    }
});