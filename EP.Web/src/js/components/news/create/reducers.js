import {
    handleActions,
    Action
} from 'redux-actions';
import {
    CREATE_NEWS,
    CREATE_NEWS_SUCCESS,
    CREATE_NEWS_FAILURE,
    PREPARE_DATA_FOR_NEWS_CREATE
} from '../types';
import {
    SELECTED_FILE,
    REMOVE_FILE
} from '../../files/types';

const initialState = {
    ingress: '',
    content: '',
    title: '',
    published: false,
    publishedDate: null,
    id: null,
    blobId: null,
    files: []
};

export default handleActions({
    [PREPARE_DATA_FOR_NEWS_CREATE]: (state, action) => {
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: true,
            blobId: action.payload[0]
        });
    },
    [CREATE_NEWS]: (state, action) => {
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: true
        });
    },
    [CREATE_NEWS_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            id: action.payload,
            redirectTo: '/news',
            error: null,
            loading: false
        });
    },
    [CREATE_NEWS_FAILURE]: (state, action) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return Object.assign({}, state, {
            id: null,
            error: error,
            loading: false
        });
    },
    [SELECTED_FILE]: (state, action) => {
        let files = state.files.slice();
        if (action.payload.single) {
            if (files.length == 0) {
                files.push(action.payload.file);
            } else {
                files[0] = action.payload.file;
            }
        } else {
            files.push(action.payload.file);
        }
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: false,
            files: files
        });
    },
    [REMOVE_FILE]: (state, action) => {
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: false,
            files: state.files.filter(el => el != action.payload)
        });
    },
}, initialState);