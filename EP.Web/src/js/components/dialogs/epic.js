import {
    combineEpics
} from 'redux-observable';
import 'rxjs';
import {
    Observable
} from 'rxjs/Observable';

import {
    ASK_TO_OPEN_CONFIRMATION
} from './types';

import {
    openConfirmation
} from './actions';


const askToOpenConfirmationEpic = action$ =>
    action$.ofType(ASK_TO_OPEN_CONFIRMATION)
    .flatMap(action => {
        return Observable.of(openConfirmation(action.payload))
    });

const epics = [
    askToOpenConfirmationEpic
];

const dialogEpic = combineEpics(...Object.values(epics));

export default dialogEpic;