import { connect } from 'preact-redux'
import { getFiles, getFilesSuccess, getFilesFailure } from '../fileActions';
import Pagination from './pagination';

const mapStateToProps = (state) => {
    return {
        fileState: state.posts.fileState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        getFiles: (page) => {
            dispatch(getFiles(page));
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Pagination);