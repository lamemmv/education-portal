import { combineReducers } from 'redux';
import PostReducer from '../components/posts/fileReducer';
import { reducer as formReducer } from 'redux-form';

const rootReducer = combineReducers({
  posts: PostReducer, //<-- Posts
  form: formReducer, 
});

export default rootReducer;
