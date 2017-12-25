import {
    handleActions,
    Action
} from 'redux-actions';

import {
    GOTO_FILES,
    GOTO_NEWS_HOME,
    GOTO_HOME,
    HANDLE_HOME_ROUTE,
    HANDLE_NEWS_HOME_ROUTE,
    HANDLE_FILES_ROUTE
} from './types';

const initialState = {
    items: [],
    loading: false,
    error: null
};

let getBreadcrumbs = (items) => {
    if (items == null || items.length == 0) {
        return [{
            name: mapResources('home'),
            path: '/'
        }];
    }

    return items.map((item) => {
        return {
            name: mapResources(item.id),
            path: item.path
        }
    });
}

let mapResources = (name) => {
    return name;
}

export default handleActions({
    [GOTO_HOME]: (state, action) => {
        return Object.assign({}, state, {
            items: [],
            error: null,
            loading: true
        });
    },
    [HANDLE_HOME_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: action.payload ? getBreadcrumbs(action.payload) : [],
            error: null,
            loading: true
        });
    },
    [GOTO_FILES]: (state, action) => {
        return Object.assign({}, state, {
            items: [],
            error: null,
            loading: true
        });
    },
    [HANDLE_FILES_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: action.payload ? getBreadcrumbs(action.payload) : [],
            error: null,
            loading: true
        });
    },
    [GOTO_NEWS_HOME]: (state, action) => {
        return Object.assign({}, state, {
            items: [],
            error: null,
            loading: false
        });
    },
    [HANDLE_NEWS_HOME_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: action.payload ? getBreadcrumbs(action.payload) : [],
            error: null,
            loading: true
        });
    },
}, initialState);