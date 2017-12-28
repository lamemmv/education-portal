import {
    combineReducers
} from 'redux';

import PostReducer from '../components/files/fileReducer';
import DeleteReducer from '../components/files/delete/reducers';
import UploadReducer from '../components/files/upload/reducers';
import NewsReducer from '../components/news/newsReducer';
import NewsCreateReducer from '../components/news/create/reducers';
import notification from '../components/notify/notification.reducer';
import breadcrumbs from '../components/breadcrumbs/reducers';

const rootReducer = combineReducers({
    posts: PostReducer,
    deleteFile: DeleteReducer,
    uploadFile: UploadReducer,
    news: NewsReducer,
    newsCreate: NewsCreateReducer,
    notification,
    breadcrumbs
});

export default rootReducer;