import {
    handleActions,
    Action
} from 'redux-actions';
import {
    GET_NEWS_LIST,
    GET_NEWS_LIST_FAILURE,
    GET_NEWS_LIST_SUCCESS,
    GET_NEWS_BYID,
    GET_NEWS_BYID_SUCCESS,
    GET_NEWS_BYID_FAILURE,
    UPDATE_NEWS,
    UPDATE_NEWS_SUCCESS,
    UPDATE_NEWS_FAILURE,
    DELETE_NEWS,
    DELETE_NEWS_SUCCESS,
    DELETE_NEWS_FAILURE,
    PREPARE_DATA_FOR_NEWS_CREATE,
    CREATE_NEWS,
    CREATE_NEWS_SUCCESS,
    CREATE_NEWS_FAILURE
} from './types';

import {
    SELECTED_FILE,
    REMOVE_FILE
} from '../files/types';

const initialState = {
    ingress: '',
    content: '',
    title: '',
    published: false,
    publishedDate: null,
    id: null,
    blobId: null,
    files: [],
    items: [],
    page: 1,
    loading: false,
    error: null
};

export default handleActions({
    [GET_NEWS_LIST]: (state, action) => {
        return Object.assign({}, state, {
            redirectTo: null,
            items: [],
            page: action.payload.page,
            error: null,
            loading: true
        });
    },
    [GET_NEWS_LIST_SUCCESS]: (state, action) => {
        let pages = [];
        if (action.payload.totalPages > 1) {
            for (let i = 0; i < action.payload.totalPages; i++) {
                pages.push(i + 1);
            }
        }
        return Object.assign({}, state, {
            redirectTo: null,
            items: action.payload.items ? action.payload.items : [],
            page: action.payload.page,
            pages: pages,
            size: action.payload.size,
            error: null,
            loading: false
        });
    },
    [GET_NEWS_LIST_FAILURE]: (state, action) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return Object.assign({}, state, {
            redirectTo: null,
            items: [],
            page: action.payload.page,
            error: error,
            loading: false
        });
    },
    [GET_NEWS_BYID]: (state, action) => {
        return Object.assign({}, state, {
            redirectTo: null,
            news: null,
            id: action.payload,
            error: null,
            loading: true
        });
    },
    [GET_NEWS_BYID_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            redirectTo: null,
            news: action.payload.data,
            id: action.payload.request,
            error: null,
            loading: false
        });
    },
    [UPDATE_NEWS]: (state, action) => {
        return Object.assign({}, state, {
            redirectTo: null,
            error: null,
            loading: true
        });
    },
    [UPDATE_NEWS_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            redirectTo: '/news',
            error: null,
            loading: false
        });
    },
    [UPDATE_NEWS_FAILURE]: (state, action) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return Object.assign({}, state, {
            redirectTo: null,
            error: error,
            loading: false
        });
    },
    [PREPARE_DATA_FOR_NEWS_CREATE]: (state, action) => {
        return Object.assign({}, state, {
            id: null,
            error: null,
            loading: true,
            blobId: action.payload.data[0].id,
            url: action.payload.request.files[0].url
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