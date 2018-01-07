import { handleActions, Action } from 'redux-actions';
import {
    GET_FOLDERS,
    GET_FOLDERS_SUCCESS,
    GET_FOLDERS_FAILURE,
    GET_FOLDER_BYID,
    GET_FOLDER_BYID_SUCCESS,
    GET_FOLDER_BYID_FAILURE,
    CREATE_FOLDER,
    CREATE_FOLDER_SUCCESS,
    CREATE_FOLDER_FAILURE,
    SHOW_CREATE_FOLDER_DIALOG,
    CLOSE_CREATE_FOLDER_DIALOG
} from './types';

const initialState = {
    items: [],
    page: 1,
    loading: false,
    error: null
};

export default handleActions({
    [GET_FOLDERS]: (state, action) => {
        return Object.assign({}, state, {
            items: [],
            page: action.payload.page,
            error: null,
            loading: true
        });
    },
    [GET_FOLDERS_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            items: action.payload.items ? action.payload.items : [],
            page: action.payload.page,
            error: null,
            loading: false
        });
    },
    [GET_FOLDERS_FAILURE]: (state, action) => {
        let error = action.payload || {
            message: action.payload.message
        }
        return Object.assign({}, state, {
            items: [],
            page: action.payload.page,
            error: error,
            loading: false
        });
    },
    [GET_FOLDERS_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            items: action.payload.items ? action.payload.items : [],
            page: action.payload.page,
            error: null,
            loading: false
        });
    },
    [SHOW_CREATE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: true,
            folderId: action.payload,
            error: null,
            loading: false
        });
    },
    [CLOSE_CREATE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: false,
            error: null,
            loading: false
        });
    },
    [CREATE_FOLDER_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: false,
            error: null,
            loading: false
        });
    },
    [CREATE_FOLDER_FAILURE]: (state, action) => {
        return Object.assign({}, state, {
            showDialog: false,
            error: null,
            loading: false
        });
    },
}, initialState);