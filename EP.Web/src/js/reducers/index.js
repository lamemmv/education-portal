import {
    combineReducers
} from 'redux';
import PostReducer from '../components/posts/fileReducer';
import DeleteReducer from '../components/posts/delete/deleteReducer';
import UploadReducer from '../components/posts/upload/uploadReducer';
import NewsReducer from '../components/news/newsReducer';

const rootReducer = combineReducers({
    posts: PostReducer,
    deleteFile: DeleteReducer,
    uploadFile: UploadReducer,
    news: NewsReducer
});

export default rootReducer;