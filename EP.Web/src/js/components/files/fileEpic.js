import 'rxjs';
import {
    Observable
} from 'rxjs/Observable';
import {
    combineEpics
} from 'redux-observable';

import {
    UPLOAD_FILE,
    UPLOAD_FILE_SUCCESS,
    GET_FILES,
    ASK_FOR_DELETING_FILE,
    DELETE_FILE,
    DELETE_FILE_SUCCESS,
} from './types';
import {
    getFiles,
    getFilesSuccess,
    getFilesFailure
} from './fileActions';
import {
    uploadFileSuccess,
    uploadFileFailure
} from './upload/uploadActions';
import {
    showModal,
    deleteFileSuccess,
    deleteFileFailure
} from './delete/deleteActions';

import {
    addNotification
} from '../notify/notification.actions';

import API from '../api';

const fetchFilesEpic = action$ =>
    action$.ofType(GET_FILES)
    .mergeMap(action =>
        Observable.fromPromise(API.getFiles(action.payload))
        .map(response => getFilesSuccess(response.data))
        .catch(error => Observable.of(getFilesFailure(error)))
    );

const uploadFilesEpic = action$ =>
    action$.ofType(UPLOAD_FILE)
    .mergeMap(action =>
        Observable.fromPromise(API.uploadFiles(action.payload))
        .map(response => uploadFileSuccess(response.data))
        .catch(error => Observable.of(uploadFileFailure(error)))
    );

const uploadFileSuccessEpic = action$ =>
    action$.ofType(UPLOAD_FILE_SUCCESS)
    .flatMap(action =>
        Observable.concat(
            Observable.of(getFiles(1)),
            Observable.of(addNotification('Uploaded', 'success', 'Upload files'))
        )
    );
    //.takeUntil(action$.ofType(LOGIN_ABORT));
    //.catch(({ xhr }) => Observable.of(loginFailed(xhr.response)))

const askForDeleteFileEpic = action$ =>
    action$.ofType(ASK_FOR_DELETING_FILE)
    .map(action => showModal(action.payload));

const confirmDeleteFilePic = action$ =>
    action$.ofType(DELETE_FILE)
    .mergeMap(action =>
        Observable.fromPromise(API.deleteFile(action.payload))
        .map(response => deleteFileSuccess(response.data))
        .catch(error => Observable.of(deleteFileFailure(error)))
    );

const deleteFileSuccessEpic = action$ =>
    action$.ofType(DELETE_FILE_SUCCESS)
    .flatMap(action =>
        Observable.of(
            getFiles(1),
            addNotification('Successful', 'success', 'Delete file')
          )
    );

const epics = [
    fetchFilesEpic,
    uploadFilesEpic,
    uploadFileSuccessEpic,
    askForDeleteFileEpic,
    confirmDeleteFilePic,
    deleteFileSuccessEpic
];

const fileEpics = combineEpics(...Object.values(epics));

export default fileEpics;