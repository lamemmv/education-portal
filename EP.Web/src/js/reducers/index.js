import {
    combineReducers
} from 'redux';
import PostReducer from '../components/files/fileReducer';
import DeleteReducer from '../components/files/delete/deleteReducer';
import UploadReducer from '../components/files/upload/uploadReducer';
import NewsReducer from '../components/news/newsReducer';
import NewsCreateReducer from '../components/news/create/news.create.reducer';
import notification from '../components/notify/notification.reducer';

const rootReducer = combineReducers({
    posts: PostReducer,
    deleteFile: DeleteReducer,
    uploadFile: UploadReducer,
    news: NewsReducer,
    newsCreate: NewsCreateReducer,
    notification
});

export default rootReducer;