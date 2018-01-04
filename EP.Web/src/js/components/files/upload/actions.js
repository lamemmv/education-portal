import {
    SELECTED_FILE,
    UPLOAD_FILE,
    UPLOAD_FILE_SUCCESS,
    UPLOAD_FILE_FAILURE,
    REMOVE_FILE
} from '../types';

import {
    createAction
} from 'redux-actions';

export const selectFile = createAction(SELECTED_FILE);
export const uploadFiles = createAction(UPLOAD_FILE);
export const uploadFileSuccess = createAction(UPLOAD_FILE_SUCCESS);
export const uploadFileFailure = createAction(UPLOAD_FILE_FAILURE);
export const removeFile = createAction(REMOVE_FILE);

// export function selectFile(file) {
//     return {
//         type: SELECTED_FILE,
//         payload: file
//     };
// }

// export function uploadFiles(request) {
//     return {
//         type: UPLOAD_FILE,
//         payload: request
//     };
// }

// export function uploadFileSuccess(id) {
//     return {
//         type: UPLOAD_FILE_SUCCESS,
//         payload: id
//     };
// }

// export function uploadFileFailure(error) {
//     return {
//         type: UPLOAD_FILE_FAILURE,
//         payload: error
//     };
// }

// export function removeFile(file) {
//     return {
//         type: REMOVE_FILE,
//         payload: file
//     };
// }