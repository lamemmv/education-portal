import 'rxjs';
import {
    Observable
} from 'rxjs/Observable';
import {
    combineEpics
} from 'redux-observable';
import {
    Localizer,
    Text
} from 'preact-i18n';

import {
    UPLOAD_FILE,
    UPLOAD_FILE_SUCCESS,
    GET_FILES,
    ASK_FOR_DELETING_FILE,
    DELETE_FILE,
    DELETE_FILE_SUCCESS,
    HIT_TO_BROWSE_FILE,
    GET_FILES_SUCCESS,
    HIT_TO_DOWNLOAD_FILE
} from './types';
import {
    getFiles,
    getFilesSuccess,
    getFilesFailure,
    downloadFile
} from './fileActions';
import {
    uploadFileSuccess,
    uploadFileFailure,
    browseFile
} from './upload/actions';
import {
    showModal,
    deleteFileSuccess,
    deleteFileFailure
} from './delete/actions';

import {
    addNotification
} from '../notify/notification.actions';

import {
    updateFileBreadcrumb,
    epicEnd
} from '../breadcrumbs/actions';

import API from '../api';
import Service from '../errorHandler';

const fetchFilesEpic = action$ =>
    action$.ofType(GET_FILES)
    .mergeMap(action =>
        Observable.fromPromise(API.getFiles(action.payload))
        .map(response => getFilesSuccess({
            request: action.payload,
            response: response.data
        }))
        .catch(error => Observable.of(getFilesFailure(error)))
    );

const fetchFilesSuccessEpic = action$ =>
    action$.ofType(GET_FILES_SUCCESS)
    .flatMap(action => {
        return action.payload.request.folderId ?
            Observable.of(updateFileBreadcrumb(action.payload.response)) :
            Observable.of(epicEnd())
    });

const uploadFilesEpic = action$ =>
    action$.ofType(UPLOAD_FILE)
    .mergeMap(action =>
        Observable.fromPromise(API.uploadFiles({
            files: action.payload.files,
            parent: action.payload.parent
        }))
        .map(response => uploadFileSuccess({
            data: response.data,
            request: action.payload
        }))
        .catch(error => Observable.of(uploadFileFailure(error)))
    );

const uploadFileSuccessEpic = action$ =>
    action$.ofType(UPLOAD_FILE_SUCCESS)
    .flatMap(action => {
        return action.payload.request.action ?
            Observable.concat(
                Observable.of(getFiles({
                    page: 1,
                    folderId: action.payload.request.parent
                })),
                Observable.of(action.payload.request.action({
                    request: action.payload.request,
                    data: action.payload.data
                })),
                Observable.of(addNotification('Uploaded', 'success', 'Upload files'))) :
            Observable.concat(
                Observable.of(getFiles({
                    page: 1,
                    folderId: action.payload.request.parent
                })),
                Observable.of(addNotification('Uploaded', 'success', 'Upload files')))
    });
//.takeUntil(action$.ofType(LOGIN_ABORT));
//.catch(({ xhr }) => Observable.of(loginFailed(xhr.response)))

const askForDeleteFileEpic = action$ =>
    action$.ofType(ASK_FOR_DELETING_FILE)
    .map(action => showModal(action.payload));

const browseFileEpic = action$ =>
    action$.ofType(HIT_TO_BROWSE_FILE)
    .map(action => browseFile(action.payload));

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

const hitToDownloadFilePic = action$ =>
    action$.ofType(HIT_TO_DOWNLOAD_FILE)
    .flatMap(action => Observable.of(downloadFile(action.payload)));

const epics = [
    fetchFilesEpic,
    fetchFilesSuccessEpic,
    uploadFilesEpic,
    uploadFileSuccessEpic,
    browseFileEpic,
    askForDeleteFileEpic,
    confirmDeleteFilePic,
    deleteFileSuccessEpic,
    hitToDownloadFilePic
];

const fileEpics = combineEpics(...Object.values(epics));

export default fileEpics;