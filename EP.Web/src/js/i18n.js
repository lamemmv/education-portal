import i18n from 'i18next';
import Backend from 'i18next-xhr-backend';
import LanguageDetector from 'i18next-browser-languagedetector';
import { reactI18nextModule } from 'react-i18next';

i18n
    .use(Backend)
    .use(LanguageDetector)
    .use(reactI18nextModule)
    .init({
        fallbackLng: {
            'en-US': ['en'],
            'vi-VN': ['vi'],
            default: ['en'],
        },
        backend: {
            loadPath: '/locales/{{lng}}/{{ns}}.json'
        },
        // have a common namespace used around the full app
        ns: ['translations'],
        defaultNS: 'translations',

        debug: true,

        interpolation: {
            escapeValue: false, // not needed for react!!
        },

        react: {
            wait: true
        }
    });


export default i18n;