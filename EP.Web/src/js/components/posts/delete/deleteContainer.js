import { connect } from 'react-redux'
import {
    closeModal,
    deleteFile,
    askForDeleting,
    deleteFileSuccess,
    deleteFileFailure
} from './deleteActions';

import {
    getFiles,
    getFilesSuccess,
    getFilesFailure
} from '../fileActions';

import DeleteModal from './deleteModal';


const mapStateToProps = (state) => {
    return {
        deleteState: state.deleteFile.deleteState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        closeModal: () => {
            dispatch(closeModal());
        },
        confirmDeleteFile: (id) => {
            dispatch(deleteFile(id)).then((response) => {
                !response.error ? dispatch(deleteFileSuccess(response.payload.data)) : dispatch(deleteFileFailure(response.payload.data));
            });
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(DeleteModal);