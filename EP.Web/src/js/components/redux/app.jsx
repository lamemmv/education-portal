import React from 'react';
import { render } from 'react-dom';
import { createStore } from 'redux';
//import { TodoList } from './components';
//import { List, Map } from 'immutable';
import reducer from './reducer';
import { Provider } from 'react-redux';
import { TodoList } from './containers';

const store = createStore(reducer);

// const dummyTodos = List([
//     Map({ id: 0, isDone: true, text: 'make components' }),
//     Map({ id: 1, isDone: false, text: 'design actions' }),
//     Map({ id: 2, isDone: false, text: 'implement reducer' }),
//     Map({ id: 3, isDone: false, text: 'connect components' })
// ]);

render(
    <Provider store={store}>
        <TodoList />
    </Provider>,
    document.getElementById('root')
);