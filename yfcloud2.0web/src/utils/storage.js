var state = window.localStorage && (window.localStorage.setItem('HNGF', 123) , window.localStorage.getItem('HNGF') == 123)
var LStorage ={
    // get:function(name){
    //     if(state){
    //         return localStorage.getItem(name);
    //     }else{
    //         return false;
    //     }
    // },
    // set:function(name,item){
    //     if(state){
    //         localStorage.setItem(name, item);
    //     }
    // },
    remove:function(name){
        if(state){
            localStorage.removeItem(name);
        }
    },
    clear:function(){
        if(state){
            localStorage.clear();
        }
    },
    removeColde:function() {
        //遍历本地存储localStorage
        for (var i = 0; i < localStorage.length; i++) {
            var key = localStorage.key(i); //获取本地存储的Key
            // console.log(key);
            if(key!='companyId'||key!='lodinData'||key!='copyList'||key!='copyList_No') {
                localStorage.clear(key);
            }
            // console.log(localStorage.getItem(key));//所有value
        }
    },
    set:function(key, value, expire){
        if(expire){
            var expire = expire;
        }else{
            let start=new Date().getTime()
            let end = new Date(new Date(new Date().toLocaleDateString()).getTime()+24*60*60*1000-1).getTime();
            var expire = end-start;
        }
        let obj={
            data:value,
            time:Date.now(),
            expire:expire
        };
        localStorage.setItem(key,JSON.stringify(obj));
    },
    get:function(key){
        let val =localStorage.getItem(key);
        if(!val){
            return val;
        }
        val =JSON.parse(val);
        if(Date.now()-val.time>val.expire){
            localStorage.removeItem(key);
            return null;
        }
        return val.data;
    }
}
var SStorage ={
   
}

export default {
    LStorage,
    SStorage
} 