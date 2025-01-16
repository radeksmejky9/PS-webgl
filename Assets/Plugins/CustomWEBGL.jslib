mergeInto(LibraryManager.library, {
    UploadQR: function(x, y, z, yRot) {
        // Use the passed parameters directly
        var xPos = x;
        var yPos = y;
        var zPos = z;
        var yRotation = yRot;

        // Call the external JavaScript function (must be globally accessible)
        if (typeof window.UploadQR === "function") {
            window.UploadQR(xPos, yPos, zPos, yRotation); // Explicitly call the global UploadQR
        } else {
            console.error("UploadQR is not defined in the hosting page.");
        }
    }
});