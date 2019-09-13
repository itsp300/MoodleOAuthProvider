const createError = require('http-errors');
const express = require('express');
const path = require('path');
const cookieParser = require('cookie-parser');
const logger = require('morgan');

const indexRouter = require('./routes/index');
const app = express();


const routes = express.Router();
const OAuthServer = require("express-oauth-server");


const oauth = require('./server.js')

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

app.use(logger('dev'));

app.use(express.json());
app.use(express.urlencoded({extended: false}));
app.use(cookieParser());
// app.use(oauth.authorize());

app.get('/', indexRouter);


const form = `<html lang = "en">
<body>
<form method = "post" action = "/oauth/authorize">

    <input type = "text" name = "response_type" value = "response_type_value"/>

    <input type = "text" name = "redirect_uri" value = "redirect_uri_value"/>

    <input type = "text" name = "scope" value = "scope_value"/>

    <input type = "text" name = "state" value = "state_value"/>

    <input type = "text" name = "client_id" value = "client_id_value"/>
    
    <input type= "text" name="username" value="username"/>
    <input type= "text" name="password" value="password"/>


    <button type = "submit">Submit</button>

</form>
</body>
</html>`;
app.get("/oauth/authorize", function (req, res, next) {
    // render an authorization form
    res.send(form.replace("response_type_value", req.query["response_type"]).replace("redirect_uri_value", req.query["redirect_uri"]).replace("scope_value", req.query["scope"]).replace("state_value", req.query["state"]).replace("client_id_value", req.query["client_id"]))
});

app.post('/oauth/authorize', (req, res, next) => {
    console.log('Initial User Authentication')
    const {username, password} = req.body
    if (username === 'username' && password === 'password') {
        req.body.user = {user: 1}
        return next()
    }
    const params = [ // Send params back down
        'client_id',
        'redirect_uri',
        'response_type',
        'grant_type',
        'state',
    ]
        .map(a => `${a}=${req.body[a]}`)
        .join('&')
    return res.redirect(`/oauth?success=false&${params}`)
}, (req, res, next) => { // sends us to our redirect with an authorization code in our url
    console.log('Authorization')
    return next()
}, oauth.authorize({
    authenticateHandler: {
        handle: req => {
            console.log('Authenticate Handler')
            console.log(Object.keys(req.body).map(k => ({name: k, value: req.body[k]})))
            return req.body.user
        }
    }
}))
app.post('/oauth/token', (req, res, next) => {
    console.log('Token')
    next()
}, oauth.token({
    requireClientAuthentication: { // whether client needs to provide client_secret
        'authorization_code': false
    },
}));  // Sends back token


app.get("/oauth/info", (req, res, next) => {
    const profile = {
        user_id: '123',
        given_name: 'Eugenio',
        family_name: 'Pace',
        email: 'eugenio@mail.com'
    };
    console.log(JSON.stringify(profile));
    res.send(JSON.stringify(profile));

});

// catch 404 and forward to error handler
app.use(function (req, res, next) {
    next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
    // set locals, only providing error in development
    res.locals.message = err.message;
    res.locals.error = req.app.get('env') === 'development' ? err : {};

    // render the error page
    res.status(err.status || 500);
    res.render('error');
});


module.exports = app;


// function(accessToken, ctx, cb) {
//     request.get("https://npm.imja.red/oauth/info", {
//         headers: {
//             "Authorization": "Bearer " + accessToken
//         }
//     }, function(e, r, b) {
//         if (e) return cb(e);
//         if (r.statusCode !== 200) return cb(new Error("status: " + r.statusCode));
//         var profile = JSON.parse(b);
//
//         cb(accessToken, profile);
//     });
// // call oauth2 APIwith the accesstoken and create the profile
//
// }