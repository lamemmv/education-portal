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
import FolderReducers from '../components/folders/reducers';

const rootReducer = combineReducers({
    posts: PostReducer,
    deleteFile: DeleteReducer,
    uploadFile: UploadReducer,
    news: NewsReducer,
    newsCreate: NewsCreateReducer,
    notification,
    breadcrumbs,
    folders: FolderReducers
});

export default rootReducer;