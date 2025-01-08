mergeInto(LibraryManager.library, {
    saveModel: function(fileIDPtr, fileContentPtr) {
        // Convert Unity's string pointers to JavaScript strings
        var fileID = UTF8ToString(fileIDPtr);
        var fileContent = UTF8ToString(fileContentPtr);

        // Call the external JavaScript function (must be globally accessible)
        if (typeof saveModel === "function") {
            saveModel(fileID, fileContent);
        } else {
            console.error("saveModel is not defined in the hosting page.");
        }
    }
});