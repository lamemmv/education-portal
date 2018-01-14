import {
    combineReducers
} from 'redux';

import PostReducer from '../components/files/fileReducer';
import DeleteReducer from '../components/files/delete/reducers';
import UploadReducer from '../components/files/upload/reducers';
import NewsReducer from '../components/news/newsReducer';
import notification from '../components/notify/notification.reducer';
import breadcrumbs from '../components/breadcrumbs/reducers';
import FolderReducers from '../components/folders/reducers';
//import ConfirmationReducers from '../components/dialogs/reducers';

const rootReducer = combineReducers({
    posts: PostReducer,
    deleteFile: DeleteReducer,
    uploadFile: UploadReducer,
    news: NewsReducer,
    notification,
    breadcrumbs,
    folders: FolderReducers,
    ///dialog: ConfirmationReducers
});

export default rootReducer;