import {
    combineEpics
} from 'redux-observable';
import {
    Observable
} from 'rxjs/Observable';
import 'rxjs';

import {
    GOTO_NEWS_HOME,
    GOTO_FILES,
    GOTO_HOME
} from './types';

import {
    gotoFiles,
    gotoHomeNews,
    gotoNewsDetail,
    handleHomeRoute,
    handleFilesRoute,
    handleNewsHomeRoute
} from './actions';

const gotoHomeEpic = (action$, store) =>
    action$.ofType(GOTO_HOME)
    .map(action => handleHomeRoute(action.payload));

const gotoNewsEpic = (action$, store) =>
    action$.ofType(GOTO_NEWS_HOME)
    .map(action => handleNewsHomeRoute(action.payload));

const gotoFilesEpic = (action$, store) =>
    action$.ofType(GOTO_FILES)
    .map(action => handleFilesRoute(action.payload));

const epics = [
    gotoNewsEpic,
    gotoFilesEpic,
    gotoHomeEpic
];

const breadcrumbsEpic = combineEpics(...Object.values(epics));

export default breadcrumbsEpic;