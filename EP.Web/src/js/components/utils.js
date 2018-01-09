const Utils = {
    getIcon(node) {
        switch (node.contentType) {
            case 'image/gif':
                return require('../../assets/images/gif.png');
            case 'image/jpeg':
            case 'image/jpg':
                return require('../../assets/images/jpg.png');
            case 'image/png':
                return require('../../assets/images/png.png');
            case 'application/pdf':
                return require('../../assets/images/pdf.png');
            case 'application/msword':
            case 'application/vnd.openxmlformats-officedocument.wordprocessingml.document':
                return require('../../assets/images/word.png');
            default:
                return require('../../assets/images/document.png');
        }
    },
}

export default Utils;