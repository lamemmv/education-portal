import { connect } from 'react-redux'
import { getFiles, getFilesSuccess, getFilesFailure } from '../fileActions';
import Pagination from './pagination';


const mapStateToProps = (state) => {
    return {
        fileState: state.posts.fileState
    };
}

const mapDispatchToProps = (dispatch) => {
    return {
        getFiles: (currentPage) => {
            dispatch(getFiles(currentPage)).then((response) => {
                !response.error ? dispatch(getFilesSuccess(response.payload.data)) : dispatch(getFilesFailure(response.payload.data));
            });
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Pagination);