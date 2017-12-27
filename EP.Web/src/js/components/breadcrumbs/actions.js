import {
    GOTO_NEWS_HOME,
    GOTO_FILES,
    GOTO_NEWS_DETAIL,
    GOTO_HOME,
    HANDLE_HOME_ROUTE,
    HANDLE_FILES_ROUTE,
    HANDLE_NEWS_HOME_ROUTE,
    HANDLE_NEWS_CREATE_ROUTE,
    HANDLE_NEWS_DETAIL_ROUTE
} from './types';

export function gotoFiles(params) {
    return {
        type: GOTO_FILES,
        payload: params
    };
}

export function gotoHomeNews(params) {
    return {
        type: GOTO_NEWS_HOME,
        payload: params
    };
}

export function gotoNewsDetail(params) {
    return {
        type: GOTO_NEWS_DETAIL,
        payload: params
    };
}

export function gotoHome(params) {
    return {
        type: GOTO_HOME,
        payload: params
    };
}

export function handleHomeRoute(params) {
    return {
        type: HANDLE_HOME_ROUTE,
        payload: params
    };
}

export function handleFilesRoute(params) {
    return {
        type: HANDLE_FILES_ROUTE,
        payload: params
    };
}

export function handleNewsHomeRoute(params) {
    return {
        type: HANDLE_NEWS_HOME_ROUTE,
        payload: params
    };
}

export function handleNewsCreateRoute(params) {
    return {
        type: HANDLE_NEWS_CREATE_ROUTE,
        payload: params
    };
}

export function handleNewsDetailRoute(params) {
    return {
        type: HANDLE_NEWS_DETAIL_ROUTE,
        payload: params
    };
}