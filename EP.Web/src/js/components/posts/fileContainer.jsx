import { connect } from 'preact-redux'
import {
    getFiles,
    getFilesSuccess,
    getFilesFailure
} from './fileActions';

import { askForDeleting } from './delete/deleteActions';

import FileList from './fileList';


const mapStateToProps = (state) => {
    return {
        fileState: state.posts.fileState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        getFiles: (page) => {
            dispatch(getFiles(page));
        },
        askForDeleting: (id) => {
            dispatch(askForDeleting(id));
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(FileList);