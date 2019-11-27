import axios from 'axios'
import { getToken } from '@/utils/auth'
import serverConfig from '@/serverConfig'

export function Heartbeat() {
    //系统心跳检测
    clearInterval(window.HealthTime)
    window.HealthTime = setInterval(() => {
        doHealth()
    }, 1000 * 60 * 5)
    doHealth()
    function doHealth() {
        if (window.document.location.hash.indexOf("login") != -1) {
            return
        }
        if (window.document.location.hash.indexOf("password") != -1) {
            return
        }
        if (window.document.location.hash.indexOf("signIn") != -1) {
            return
        }
        if (window.document.location.hash.indexOf("termsService") != -1) {
            return
        }
        if (!getToken()) {
            return
        }
        axios({
            method: 'post',
            headers: {
                'Content-Type': 'application/json',
                'x-token': getToken() || ''
            },
            url: serverConfig.production+'/logstatus/api/Health'
        }).then((res) => {
            // console.log(res.data)
        })
    }
}