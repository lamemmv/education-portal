import {
    createAction
} from 'redux-actions';

import {
    ASK_TO_OPEN_CONFIRMATION,
    OPEN_CONFIRMATION,
    CLOSE_CONFIRMATION
} from './types';

export const askToOpenConfirmation = createAction(ASK_TO_OPEN_CONFIRMATION);
export const openConfirmation = createAction(OPEN_CONFIRMATION);
export const closeConfirmation = createAction(CLOSE_CONFIRMATION);