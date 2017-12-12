import {
  combineReducers
} from 'redux';
import PostReducer from '../components/posts/fileReducer';
import DeleteReducer from '../components/posts/delete/deleteReducer';
import UploadReducer from '../components/posts/upload/uploadReducer';
import {
  reducer as formReducer
} from 'redux-form';

const rootReducer = combineReducers({
  posts: PostReducer, //<-- Posts
  form: formReducer,
  deleteFile: DeleteReducer,
  uploadFile: UploadReducer
});

export default rootReducer;