import { h, Component } from 'preact';
import ShortNews from './shortNews/index';
import EPActivities from './activities/index';
import SkeletonScreen from '../skeleton-screen/theme-one';
import PlaceholderContent from '../skeleton-screen/theme-two';
import * as styles from './styles.css';

class Home extends Component {
    render() {
        return (
            <div>
                <ShortNews />
                <EPActivities />
            </div>
        );
    }
}

export default Home;