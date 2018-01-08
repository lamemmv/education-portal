import {
    createAction
} from 'redux-actions';

import {
    GOTO_NEWS_HOME,
    HANDLE_NEWS_HOME_ROUTE,
    GOTO_NEWS_DETAIL,
    HANDLE_NEWS_DETAIL_ROUTE,
    GOTO_HOME,
    HANDLE_HOME_ROUTE,
    GOTO_FILES,
    HANDLE_FILES_ROUTE,
    GOTO_NEWS_CREATE,
    HANDLE_NEWS_CREATE_ROUTE,
    GOTO_FILE_PREVIEW,
    HANDLE_FILE_PREVIEW_ROUTE,
    UPDATE_FILES_BREADCRUMB,
    EPIC_END
} from './types';


export const gotoFiles = createAction(GOTO_FILES);
export const handleFilesRoute = createAction(HANDLE_FILES_ROUTE);
export const gotoHomeNews = createAction(GOTO_NEWS_HOME);
export const handleNewsHomeRoute = createAction(HANDLE_NEWS_HOME_ROUTE);
export const gotoNewsDetail = createAction(GOTO_NEWS_DETAIL);
export const handleNewsDetailRoute = createAction(HANDLE_NEWS_DETAIL_ROUTE);
export const gotoNewsCreate = createAction(GOTO_NEWS_CREATE);
export const handleNewsCreateRoute = createAction(HANDLE_NEWS_CREATE_ROUTE);
export const gotoHome = createAction(GOTO_HOME);
export const handleHomeRoute = createAction(HANDLE_HOME_ROUTE);
export const gotoFilePreview = createAction(GOTO_FILE_PREVIEW);
export const handleFilePreviewRoute = createAction(HANDLE_FILE_PREVIEW_ROUTE);
export const updateFileBreadcrumb = createAction(UPDATE_FILES_BREADCRUMB);
export const epicEnd = createAction(EPIC_END);