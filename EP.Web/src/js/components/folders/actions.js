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
    CREATE_FOLDER_FAILURE
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