import {
  combineReducers
} from 'redux';
import PostReducer from '../components/posts/fileReducer';
import DeleteReducer from '../components/posts/delete/deleteReducer';
import UploadReducer from '../components/posts/upload/uploadReducer';
import NewsReducer from '../components/news/newsReducer';
import {
  reducer as formReducer
} from 'redux-form';

const rootReducer = combineReducers({
  // posts: PostReducer,
  // form: formReducer,
  // deleteFile: DeleteReducer,
  // uploadFile: UploadReducer,
  news: NewsReducer
});

export default rootReducer;