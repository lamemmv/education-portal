import { h, Component } from 'preact';
import { Provider } from 'preact-redux';
import EPHeader from './header';
import EPMain from './main';

const App = ({ store }) => (
    <Provider store={store}>
        <div>
            <EPHeader />
            <EPMain />
        </div>
    </Provider>
)

export default App;