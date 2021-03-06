import 'rxjs';
import {
    Observable
} from 'rxjs/Observable';
import {
    combineEpics
} from 'redux-observable';

import {
    GET_FOLDERS,
    GET_FOLDER_BYID,
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
    ASK_TO_SHOW_UPDATE_FOLDER_DIALOG,
    ASK_TO_SHOW_DELETE_FOLDER_DIALOG,
} from './types';

import {
    getFoldersSuccess,
    getFoldersFailure,
    getFolderByIdSuccess,
    getFolderByIdFailure,
    createFolderSuccess,
    createFolderFailure,
    showCreateFolderDialog,
    closeCreateFolderDialog,
    showUpdateFolderDialog,
    closeUpdateFolderDialog,
    updateFolderSuccess,
    updateFolderFailure,
    deleteFolderSuccess,
    deleteFolderFailure,
    showDeleteFolderDialog,
    closeDeleteFolderDialog,
} from './actions';

import {
    addNotification
} from '../notify/notification.actions';

import API from '../api';
import Service from '../errorHandler';
import {
    ASK_FOR_DELETING_FILE
} from '../files/types';

const getFoldersEpic = (action$, store) =>
    action$.ofType(GET_FOLDERS)
    .mergeMap((action) =>
        Observable.fromPromise(API.getFolders(action.payload.page, action.payload.size))
        .map(response => getFoldersSuccess(response.data))
        .catch(error => Observable.of(getFoldersFailure(error)))
    );

const getFolderByIdEpic = (action$, store) =>
    action$.ofType(GET_FOLDER_BYID)
    .mergeMap((action) =>
        Observable.fromPromise(API.getFolderById(action.payload))
        .map(response => getFolderByIdSuccess(response.data))
        .catch(error => Observable.of(getFolderByIdFailure(error)))
    );

const askForShowingCreateFolderEpic = action$ =>
    action$.ofType(ASK_TO_SHOW_CREATE_FOLDER_DIALOG)
    .map(action => showCreateFolderDialog(action.payload));

const createFolderEpic = (action$, store) =>
    action$.ofType(CREATE_FOLDER)
    .mergeMap((action) =>
        Observable.fromPromise(API.createFolder(action.payload.request))
        .map(response => createFolderSuccess(action.payload))
        .catch(error => Observable.of(createFolderFailure(error)))
    );

const createFolderSuccessEpic = action$ =>
    action$.ofType(CREATE_FOLDER_SUCCESS)
    .flatMap(action => {
        return action.payload.action ?
            Observable.concat(
                Observable.of(action.payload.action({
                    page: 1,
                    folderId: action.payload.request.parent
                })),
                Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Create folder'))) :
            Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Create folder'))
    });

const createFolderFailEpic = action$ =>
    action$.ofType(CREATE_FOLDER_FAILURE)
    .map(action => addNotification(Service.getErrorMessage(action.payload), 'error', 'Create folder'));

const askForShowingUpdateFolderEpic = action$ =>
    action$.ofType(ASK_TO_SHOW_UPDATE_FOLDER_DIALOG)
    .map(action => showUpdateFolderDialog(action.payload));

const updateFolderEpic = (action$, store) =>
    action$.ofType(UPDATE_FOLDER)
    .mergeMap((action) =>
        Observable.fromPromise(API.updateFolder(action.payload.request))
        .map(response => updateFolderSuccess(action.payload))
        .catch(error => Observable.of(updateFolderFailure(error)))
    );

const updateFolderSuccessEpic = action$ =>
    action$.ofType(UPDATE_FOLDER_SUCCESS)
    .flatMap(action => {
        return action.payload.action ?
            Observable.concat(
                Observable.of(action.payload.action({
                    page: 1,
                    folderId: action.payload.request.parent
                })),
                Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Upload folder'))) :
            Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Upload folder'))
    });

const updateFolderFailEpic = action$ =>
    action$.ofType(UPDATE_FOLDER_FAILURE)
    .map(action => addNotification(Service.getErrorMessage(action.payload), 'error', 'Update folder'));

const askToDeleteFolderEpic = action$ =>
    action$.ofType(ASK_TO_SHOW_DELETE_FOLDER_DIALOG)
    .map(action => showDeleteFolderDialog(action.payload));

const deleteFoldersEpic = (action$, store) =>
    action$.ofType(DELETE_FOLDER)
    .mergeMap((action) =>
        Observable.fromPromise(API.deleteFolders(action.payload.request.nodes))
        .map(response => deleteFolderSuccess({
            request: action.payload,
            response: response.data
        }))
        .catch(error => Observable.of(deleteFolderFailure(error)))
    );

const deleteFoldersSuccessEpic = action$ =>
    action$.ofType(DELETE_FOLDER_SUCCESS)
    .flatMap(action => {
        return action.payload.request.action ?
            Observable.concat(
                Observable.of(action.payload.request.action({
                    page: 1,
                    folderId: action.payload.request.request.parent
                })),
                Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Delete folder'))) :
            Observable.of(addNotification(Service.getMessage('succeed'), 'success', 'Delete folder'))
    });

const deleteFoldersFailEpic = action$ =>
    action$.ofType(DELETE_FOLDER_FAILURE)
    .map(action => addNotification(Service.getErrorMessage(action.payload), 'error', 'Delete folder'));

const epics = [
    getFolderByIdEpic,
    getFoldersEpic,
    askForShowingCreateFolderEpic,
    createFolderEpic,
    createFolderSuccessEpic,
    createFolderFailEpic,
    askForShowingUpdateFolderEpic,
    updateFolderEpic,
    updateFolderSuccessEpic,
    updateFolderFailEpic,
    askToDeleteFolderEpic,
    deleteFoldersEpic,
    deleteFoldersSuccessEpic,
    deleteFoldersFailEpic
];

const foldersEpic = combineEpics(...Object.values(epics));

export default foldersEpic;