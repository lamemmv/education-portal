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
    ASK_TO_SHOW_CREATE_FOLDER_DIALOG
} from './types';

import {
    getFoldersSuccess,
    getFoldersFailure,
    getFolderByIdSuccess,
    getFolderByIdFailure,
    createFolderSuccess,
    createFolderFailure,
    showCreateFolderDialog,
    closeCreateFolderDialog
} from './actions';

import { getFiles } from '../files/fileActions';
import {
    addNotification
} from '../notify/notification.actions';

import API from '../api';

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

const createFolderEpic = (action$, store) =>
    action$.ofType(CREATE_FOLDER)
    .mergeMap((action) =>
        Observable.fromPromise(API.createFolder(action.payload.request))
        .map(response => createFolderSuccess(action.payload))
        .catch(error => Observable.of(createFolderFailure(error)))
    );

const askForShowingCreateFolderEpic = action$ =>
    action$.ofType(ASK_TO_SHOW_CREATE_FOLDER_DIALOG)
    .map(action => showCreateFolderDialog(action.payload));

const createFolderSuccessEpic = action$ =>
    action$.ofType(CREATE_FOLDER_SUCCESS)
    .flatMap(action => {
        return action.payload.action ?
            Observable.concat(
                Observable.of(action.payload.action({
                    page: 1,
                    folderId: action.payload.request.parent
                })),
                Observable.of(addNotification('Saved', 'success', 'Create folder'))) :
            Observable.of(addNotification('Saved', 'success', 'Create folder'))
    });

const epics = [
    askForShowingCreateFolderEpic,
    getFolderByIdEpic,
    getFoldersEpic,
    createFolderEpic,
    createFolderSuccessEpic
];

const foldersEpic = combineEpics(...Object.values(epics));

export default foldersEpic;