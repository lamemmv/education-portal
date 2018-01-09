import {
    createAction
} from 'redux-actions';

import {
    GET_FILES,
    GET_FILES_SUCCESS,
    GET_FILES_FAILURE,
    SELECT_FOLDER,
    DESELECT_FOLDER
} from './types';

export const getFiles = createAction(GET_FILES);
export const getFilesSuccess = createAction(GET_FILES_SUCCESS);
export const getFilesFailure = createAction(GET_FILES_FAILURE);
export const selectFolder = createAction(SELECT_FOLDER);
export const deselectFolder = createAction(DESELECT_FOLDER);