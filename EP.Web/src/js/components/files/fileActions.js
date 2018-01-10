import {
    createAction
} from 'redux-actions';

import {
    GET_FILES,
    GET_FILES_SUCCESS,
    GET_FILES_FAILURE,
    SELECT_FOLDER,
    DESELECT_FOLDER,
    HIT_TO_DOWNLOAD_FILE,
    DOWNLOAD_FILE
} from './types';

export const getFiles = createAction(GET_FILES);
export const getFilesSuccess = createAction(GET_FILES_SUCCESS);
export const getFilesFailure = createAction(GET_FILES_FAILURE);
export const selectFolder = createAction(SELECT_FOLDER);
export const deselectFolder = createAction(DESELECT_FOLDER);
export const hitToDownloadFile = createAction(HIT_TO_DOWNLOAD_FILE);
export const downloadFile = createAction(DOWNLOAD_FILE);