import * as constants from './fileActions';

/** 
The initial state is used to define your reducer. 
Usually you would just set this to default values and empty strings. 
The reason this is needed is so that when using these values you are guaranteed to at least have some default value. 
Think of it as the default constructor.
**/
const initialState = {
    id:null,
    isSaved: false
}


/**
This action part is the part that will "listen" for emitted actions. 
So the uploadFile, previewFile and deleteFile functions that we defined earlier will be handled in here. 
The action parameter is what is being returned (the type and payload) in the functions above.
**/
function name(state=initialState,action){
    switch (action.type){
  /**
  in REDUX the state is immutable. 
  You must always return a new one, which is why use the ES6 spread operator to copy the values from the states that's passed in.
  **/
      case constants.UPLOAD_FILE:
         return {
           ...state, 
           id:action.payload.id
        }
       case constants.PREVIEW_FILE:
         return {
             ...state
         }
        case constants.DELETE_FILE:
         return {
             ...state
         }
     }
  }

  export default name;