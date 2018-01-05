import {
    SELECTED_FILE,
    UPLOAD_FILE,
    UPLOAD_FILE_SUCCESS,
    UPLOAD_FILE_FAILURE,
    REMOVE_FILE,
    BROWSE_FILE,
    HIT_TO_BROWSE_FILE
} from '../types';

import {
    createAction
} from 'redux-actions';

export const selectFile = createAction(SELECTED_FILE);
export const uploadFiles = createAction(UPLOAD_FILE);
export const uploadFileSuccess = createAction(UPLOAD_FILE_SUCCESS);
export const uploadFileFailure = createAction(UPLOAD_FILE_FAILURE);
export const removeFile = createAction(REMOVE_FILE);
export const browseFile = createAction(BROWSE_FILE);
export const hitToBrowseFile = createAction(HIT_TO_BROWSE_FILE);