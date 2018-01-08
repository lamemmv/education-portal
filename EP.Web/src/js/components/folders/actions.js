import {
    createAction
} from 'redux-actions';

import {
    GET_FOLDER_BYID,
    GET_FOLDER_BYID_SUCCESS,
    GET_FOLDER_BYID_FAILURE,
    GET_FOLDERS,
    GET_FOLDERS_SUCCESS,
    GET_FOLDERS_FAILURE,
    CREATE_FOLDER,
    CREATE_FOLDER_SUCCESS,
    CREATE_FOLDER_FAILURE,
    UPDATE_FOLDER,
    UPDATE_FOLDER_SUCCESS,
    UPDATE_FOLDER_FAILURE,
    DELETE_FOLDER,
    DELETE_FOLDER_SUCCESS,
    DELETE_FOLDER_FAILURE,
    ASK_TO_SHOW_CREATE_FOLDER_DIALOG,
    CLOSE_CREATE_FOLDER_DIALOG,
    SHOW_CREATE_FOLDER_DIALOG,
    ASK_TO_SHOW_UPDATE_FOLDER_DIALOG,
    SHOW_UPDATE_FOLDER_DIALOG,
    CLOSE_UPDATE_FOLDER_DIALOG,
    ASK_TO_SHOW_DELETE_FOLDER_DIALOG,
    SHOW_DELETE_FOLDER_DIALOG,
    CLOSE_DELETE_FOLDER_DIALOG
} from './types';

export const getFolderById = createAction(GET_FOLDER_BYID);
export const getFolderByIdSuccess = createAction(GET_FOLDER_BYID_SUCCESS);
export const getFolderByIdFailure = createAction(GET_FOLDER_BYID_FAILURE);
export const getFolders = createAction(GET_FOLDERS);
export const getFoldersSuccess = createAction(GET_FOLDERS_SUCCESS);
export const getFoldersFailure = createAction(GET_FOLDERS_FAILURE);
export const createFolder = createAction(CREATE_FOLDER);
export const createFolderSuccess = createAction(CREATE_FOLDER_SUCCESS);
export const createFolderFailure = createAction(CREATE_FOLDER_FAILURE);
export const updateFolder = createAction(UPDATE_FOLDER);
export const updateFolderSuccess = createAction(UPDATE_FOLDER_SUCCESS);
export const updateFolderFailure = createAction(UPDATE_FOLDER_FAILURE);
export const deleteFolder = createAction(DELETE_FOLDER);
export const deleteFolderSuccess = createAction(DELETE_FOLDER_SUCCESS);
export const deleteFolderFailure = createAction(DELETE_FOLDER_FAILURE);
export const askToShowCreateFolderDialog = createAction(ASK_TO_SHOW_CREATE_FOLDER_DIALOG);
export const closeCreateFolderDialog = createAction(CLOSE_CREATE_FOLDER_DIALOG);
export const showCreateFolderDialog = createAction(SHOW_CREATE_FOLDER_DIALOG);
export const askToShowUpdateFolderDialog = createAction(ASK_TO_SHOW_UPDATE_FOLDER_DIALOG);
export const closeUpdateFolderDialog = createAction(CLOSE_UPDATE_FOLDER_DIALOG);
export const showUpdateFolderDialog = createAction(SHOW_UPDATE_FOLDER_DIALOG);
export const askToShowDeleteFolderDialog = createAction(ASK_TO_SHOW_DELETE_FOLDER_DIALOG);
export const closeDeleteFolderDialog = createAction(CLOSE_DELETE_FOLDER_DIALOG);
export const showDeleteFolderDialog = createAction(SHOW_DELETE_FOLDER_DIALOG);