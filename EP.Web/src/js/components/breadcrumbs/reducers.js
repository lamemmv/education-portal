import {
    handleActions,
    Action
} from 'redux-actions';

import {
    HANDLE_HOME_ROUTE,
    HANDLE_NEWS_HOME_ROUTE,
    HANDLE_FILES_ROUTE,
    HANDLE_NEWS_CREATE_ROUTE,
    HANDLE_NEWS_DETAIL_ROUTE,
    HANDLE_FILE_PREVIEW_ROUTE,
    UPDATE_FILES_BREADCRUMB
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
            path: '/',
            active: true,
            icon: '&#xe88a;'
        }];
    }

    let length = items.length;
    return items.map((item, i) => {
        return {
            name: mapResources(item.id),
            path: item.path,
            active: length == (i + 1),
            icon: mapIcon(item.id)
        }
    });
}

let mapResources = (name) => {
    switch (name) {
        case 'home':
            return 'header.home';
        case 'files':
            return 'header.files';
        case 'news':
            return 'header.news';
        case 'createNews':
            return 'news.createNews';
        case 'detailNews':
            return 'news.detail';
        default:
            return name;
    }
}

let mapIcon = (name) => {
    switch (name) {
        case 'files':
            return 'event_note';
        case 'news':
            return 'cast';
        case 'createNews':
            return 'playlist_add';
        case 'detailNews':
            return 'details';
        case 'home':
            return 'home';
        default:
            return 'folder_open';
    }
}

export default handleActions({
    [HANDLE_HOME_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                id: 'home',
                path: '/'
            }]),
            error: null,
            loading: false
        });
    },
    [HANDLE_FILES_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                    id: 'home',
                    path: '/'
                },
                {
                    id: 'files',
                    path: '/files'
                }
            ]),
            error: null,
            loading: false
        });
    },
    [HANDLE_NEWS_HOME_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                    id: 'home',
                    path: '/'
                },
                {
                    id: 'news',
                    path: '/news'
                }
            ]),
            error: null,
            loading: false
        });
    },
    [HANDLE_NEWS_CREATE_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                    id: 'home',
                    path: '/'
                },
                {
                    id: 'news',
                    path: '/news'
                },
                {
                    id: 'createNews',
                    path: '/news/create'
                }
            ]),
            error: null,
            loading: false
        });
    },
    [HANDLE_NEWS_DETAIL_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                    id: 'home',
                    path: '/'
                },
                {
                    id: 'news',
                    path: '/news'
                },
                {
                    id: 'detailNews',
                    path: action.payload
                }
            ]),
            error: null,
            loading: false
        });
    },
    [HANDLE_FILE_PREVIEW_ROUTE]: (state, action) => {
        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                    id: 'home',
                    path: '/'
                },
                {
                    id: 'files',
                    path: '/files'
                },
                {
                    id: 'filePreview',
                    path: action.payload.path
                }
            ]),
            error: null,
            loading: false
        });
    },
    [UPDATE_FILES_BREADCRUMB]: (state, action) => {
        let parents = action.payload.blob.ancestors;
        let folders = [];
        if (parents) {
            parents.map((folder) => {
                folders.push({
                    id: folder.name,
                    path: `/files/${folder.id}`
                });
            });
        }

        folders.push({
            id: action.payload.blob.name,
            path: `/files/${action.payload.blob.id}`
        });

        return Object.assign({}, state, {
            items: getBreadcrumbs([{
                    id: 'home',
                    path: '/'
                },
                {
                    id: 'files',
                    path: '/files'
                },
                ...folders
            ]),
            error: null,
            loading: false
        });
    },
}, initialState);