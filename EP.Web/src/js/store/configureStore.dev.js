import { createStore, applyMiddleware, compose } from 'redux';
import reducer from '../reducers';
import { createEpicMiddleware } from 'redux-observable';
import epics from '../epics';

const epicMiddleware = createEpicMiddleware(epics)

export default function configureStore(initialState) {
    const finalCreateStore = compose(
        applyMiddleware(epicMiddleware),
        window.devToolsExtension ? window.devToolsExtension() : f => f
    )(createStore);

    const store = finalCreateStore(reducer, initialState);

    if (module.hot) {
        // Enable Webpack hot module replacement for reducers
        module.hot.accept('../reducers', () => {
            const nextReducer = require('../reducers');
            store.replaceReducer(nextReducer);
        });
    }

    return store;
}

// Middleware you want to use in production:
// const enhancer = applyMiddleware(epicMiddleware);

// export default function configureStore(initialState) {
//   // Note: only Redux >= 3.1.0 supports passing enhancer as third argument.
//   // See https://github.com/rackt/redux/releases/tag/v3.1.0
//   return createStore(reducer, initialState, enhancer);
// };