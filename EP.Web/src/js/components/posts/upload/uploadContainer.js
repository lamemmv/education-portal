import {
    connect
} from 'react-redux'
import {
    selectFile,
    uploadFile,
    uploadFileSuccess,
    uploadFileFailure,
    removeFile
} from './uploadActions';

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
            dispatch(uploadFile(files)).then((response) => {
                !response.error ?
                    dispatch(uploadFileSuccess(response.payload.data)) :
                    dispatch(uploadFileFailure(response.payload.data));
            });
        },
        removeFile: (file) => {
            dispatch(removeFile(file))
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Upload);