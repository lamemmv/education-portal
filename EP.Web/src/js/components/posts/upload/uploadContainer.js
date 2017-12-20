import {
    connect
} from 'preact-redux'
import {
    selectFile,
    uploadFiles,
    uploadFileSuccess,
    uploadFileFailure,
    removeFile
} from './uploadActions';

import {
    getFiles,
    getFilesSuccess,
    getFilesFailure
} from '../fileActions';

import Upload from './upload';

const mapStateToProps = (state) => {
    return {
        uploadState: state.uploadFile.uploadState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        selectFile: (file) => {
            dispatch(selectFile(file));
        },
        uploadFile: (files) => {
            dispatch(uploadFiles(files));
        },
        removeFile: (file) => {
            dispatch(removeFile(file))
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Upload);