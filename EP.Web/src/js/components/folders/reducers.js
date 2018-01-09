import { handleActions, Action } from 'redux-actions';
import {
    GET_FOLDERS,
    GET_FOLDERS_SUCCESS,
    GET_FOLDERS_FAILURE,
    GET_FOLDER_BYID,
    GET_FOLDER_BYID_SUCCESS,
    GET_FOLDER_BYID_FAILURE,
    CREATE_FOLDER_SUCCESS,
    CREATE_FOLDER_FAILURE,
    UPDATE_FOLDER_SUCCESS,
    UPDATE_FOLDER_FAILURE,
    DELETE_FOLDER_SUCCESS,
    DELETE_FOLDER_FAILURE,
    SHOW_CREATE_FOLDER_DIALOG,
    CLOSE_CREATE_FOLDER_DIALOG,
    SHOW_UPDATE_FOLDER_DIALOG,
    CLOSE_UPDATE_FOLDER_DIALOG,
    SHOW_DELETE_FOLDER_DIALOG,
    CLOSE_DELETE_FOLDER_DIALOG
} from './types';

const initialState = {
    items: [],
    page: 1,
    loading: false,
    error: null,
    request: {}
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
            showCreateFolderDialog: true,
            folderId: action.payload,
            error: null,
            loading: false
        });
    },
    [CLOSE_CREATE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showCreateFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [CREATE_FOLDER_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            showCreateFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [CREATE_FOLDER_FAILURE]: (state, action) => {
        return Object.assign({}, state, {
            showCreateFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [SHOW_UPDATE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showUpdateFolderDialog: true,
            request: action.payload,
            error: null,
            loading: false
        });
    },
    [CLOSE_UPDATE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showUpdateFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [UPDATE_FOLDER_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            showUpdateFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [UPDATE_FOLDER_FAILURE]: (state, action) => {
        return Object.assign({}, state, {
            showUpdateFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [SHOW_DELETE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showDeleteFolderDialog: true,
            request: action.payload,
            error: null,
            loading: false
        });
    },
    [CLOSE_DELETE_FOLDER_DIALOG]: (state, action) => {
        return Object.assign({}, state, {
            showDeleteFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [DELETE_FOLDER_SUCCESS]: (state, action) => {
        return Object.assign({}, state, {
            showDeleteFolderDialog: false,
            error: null,
            loading: false
        });
    },
    [DELETE_FOLDER_FAILURE]: (state, action) => {
        return Object.assign({}, state, {
            showDeleteFolderDialog: false,
            error: null,
            loading: false
        });
    },
}, initialState);