import {
    combineEpics
} from 'redux-observable';

import {
    GET_FILES,
    getFilesSuccess,
    getFilesFailure
} from './fileActions';

import {
    UPLOAD_FILE,
    uploadFileSuccess,
    uploadFileFailure
} from './upload/uploadActions';

import {
    ASK_FOR_DELETING_FILE,
    DELETE_FILE,
    showModal,
    deleteFileSuccess,
    deleteFileFailure
} from './delete/deleteActions';

import API from '../api';

import 'rxjs';
import {
    Observable
} from 'rxjs/Observable'

const fetchFilesEpic = action$ =>
    action$.ofType(GET_FILES)
    .mergeMap(action =>
        Observable.fromPromise(API.getFiles(action.payload))
        .map(response => getFilesSuccess(response.data))
        .catch(error => Observable.of(getFilesFailure(error)))
    )

const uploadFilesEpic = action$ =>
    action$.ofType(UPLOAD_FILE)
    .mergeMap(action =>
        Observable.fromPromise(API.uploadFiles(action.payload))
        .map(response => uploadFileSuccess(response.data))
        .catch(error => Observable.of(uploadFileFailure(error)))
    )

const askForDeleteFileEpic = action$ =>
    action$.ofType(ASK_FOR_DELETING_FILE)
    .map(action => showModal(action.payload));

const confirmDeleteFilePic = action$ =>
    action$.ofType(DELETE_FILE)
    .mergeMap(action =>
        Observable.fromPromise(API.deleteFile(action.payload))
        .map(response => deleteFileSuccess(response.data))
        .catch(error => Observable.of(deleteFileFailure(error)))
    )


const epics = [
    fetchFilesEpic,
    uploadFilesEpic,
    askForDeleteFileEpic,
    confirmDeleteFilePic
];

const fileEpics = combineEpics(...Object.values(epics));

export default fileEpics;