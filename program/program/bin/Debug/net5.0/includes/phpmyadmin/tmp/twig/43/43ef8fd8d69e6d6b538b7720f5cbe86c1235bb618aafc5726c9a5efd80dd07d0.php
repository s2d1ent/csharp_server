<?php

use Twig\Environment;
use Twig\Error\LoaderError;
use Twig\Error\RuntimeError;
use Twig\Extension\SandboxExtension;
use Twig\Markup;
use Twig\Sandbox\SecurityError;
use Twig\Sandbox\SecurityNotAllowedTagError;
use Twig\Sandbox\SecurityNotAllowedFilterError;
use Twig\Sandbox\SecurityNotAllowedFunctionError;
use Twig\Source;
use Twig\Template;

/* javascript/variables.twig */
class __TwigTemplate_a2770bc5c704b0ecdad13de8d82d838a5a8e7f2551da0349b11acbba3c5209af extends \Twig\Template
{
    private $source;
    private $macros = [];

    public function __construct(Environment $env)
    {
        parent::__construct($env);

        $this->source = $this->getSourceContext();

        $this->parent = false;

        $this->blocks = [
        ];
    }

    protected function doDisplay(array $context, array $blocks = [])
    {
        $macros = $this->macros;
        // line 2
        echo "var firstDayOfCalendar = '";
        echo twig_escape_filter($this->env, ($context["first_day_of_calendar"] ?? null), "js", null, true);
        echo "';
var themeImagePath = '";
        // line 3
        echo twig_escape_filter($this->env, ($context["theme_image_path"] ?? null), "js", null, true);
        echo "';
var mysqlDocTemplate = '";
        // line 4
        echo twig_escape_filter($this->env, PhpMyAdmin\Util::getMySQLDocuURL("%s"), "js", null, true);
        echo "';
var maxInputVars = ";
        // line 5
        echo twig_escape_filter($this->env, ($context["max_input_vars"] ?? null), "js", null, true);
        echo ";

";
        // line 7
        ob_start(function () { return ''; });
        // line 8
        // l10n: Month-year order for calendar, use either "calendar-month-year" or "calendar-year-month".
        echo _gettext("calendar-month-year");
        $context["show_month_after_year"] = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        // line 10
        ob_start(function () { return ''; });
        // line 11
        // l10n: Year suffix for calendar, "none" is empty.
        echo _gettext("none");
        $context["year_suffix"] = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        // line 14
        echo "if (\$.datepicker) {
  \$.datepicker.regional[''].closeText = '";
        // line 15
        ob_start(function () { return ''; });
        // l10n: Display text for calendar close link
        echo _gettext("Done");
        $___internal_476115f74b21cea817768267c97d5c6faa8bb6335c2dee1d76b5fcaa0f4f7efe_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_476115f74b21cea817768267c97d5c6faa8bb6335c2dee1d76b5fcaa0f4f7efe_, "js");
        echo "';
  \$.datepicker.regional[''].prevText = '";
        // line 16
        ob_start(function () { return ''; });
        // l10n: Previous month. Display text for previous month link in calendar
        echo _gettext("Prev");
        $___internal_8248bdd332f83a27422ee81979773d503fe322565f206bcb129c77879058b5c8_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_8248bdd332f83a27422ee81979773d503fe322565f206bcb129c77879058b5c8_, "js");
        echo "';
  \$.datepicker.regional[''].nextText = '";
        // line 17
        ob_start(function () { return ''; });
        // l10n: Next month. Display text for next month link in calendar
        echo _gettext("Next");
        $___internal_376ff714cf43cb3f4b03e69a1c3f036e485af7e83de02c6d4ca9493471636ac2_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_376ff714cf43cb3f4b03e69a1c3f036e485af7e83de02c6d4ca9493471636ac2_, "js");
        echo "';
  \$.datepicker.regional[''].currentText = '";
        // line 18
        ob_start(function () { return ''; });
        // l10n: Display text for current month link in calendar
        echo _gettext("Today");
        $___internal_2a564af21bdac0f444786aed1677d76c56bc27598ddb1fa0b8a2f59198baf5f9_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_2a564af21bdac0f444786aed1677d76c56bc27598ddb1fa0b8a2f59198baf5f9_, "js");
        echo "';
  \$.datepicker.regional[''].monthNames = [
    '";
        // line 20
        echo twig_escape_filter($this->env, _gettext("January"), "js", null, true);
        echo "',
    '";
        // line 21
        echo twig_escape_filter($this->env, _gettext("February"), "js", null, true);
        echo "',
    '";
        // line 22
        echo twig_escape_filter($this->env, _gettext("March"), "js", null, true);
        echo "',
    '";
        // line 23
        echo twig_escape_filter($this->env, _gettext("April"), "js", null, true);
        echo "',
    '";
        // line 24
        echo twig_escape_filter($this->env, _gettext("May"), "js", null, true);
        echo "',
    '";
        // line 25
        echo twig_escape_filter($this->env, _gettext("June"), "js", null, true);
        echo "',
    '";
        // line 26
        echo twig_escape_filter($this->env, _gettext("July"), "js", null, true);
        echo "',
    '";
        // line 27
        echo twig_escape_filter($this->env, _gettext("August"), "js", null, true);
        echo "',
    '";
        // line 28
        echo twig_escape_filter($this->env, _gettext("September"), "js", null, true);
        echo "',
    '";
        // line 29
        echo twig_escape_filter($this->env, _gettext("October"), "js", null, true);
        echo "',
    '";
        // line 30
        echo twig_escape_filter($this->env, _gettext("November"), "js", null, true);
        echo "',
    '";
        // line 31
        echo twig_escape_filter($this->env, _gettext("December"), "js", null, true);
        echo "',
  ];
  \$.datepicker.regional[''].monthNamesShort = [
    '";
        // line 34
        ob_start(function () { return ''; });
        // l10n: Short month name for January
        echo _gettext("Jan");
        $___internal_5c733ef7546e4798398577c000fe10e515a04d745050f246c3727f9bb55e3c40_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_5c733ef7546e4798398577c000fe10e515a04d745050f246c3727f9bb55e3c40_, "js");
        echo "',
    '";
        // line 35
        ob_start(function () { return ''; });
        // l10n: Short month name for February
        echo _gettext("Feb");
        $___internal_652c2a7e17eae92afbd049684626125f5d3d9a54ddf02e3d45779af07bbd8150_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_652c2a7e17eae92afbd049684626125f5d3d9a54ddf02e3d45779af07bbd8150_, "js");
        echo "',
    '";
        // line 36
        ob_start(function () { return ''; });
        // l10n: Short month name for March
        echo _gettext("Mar");
        $___internal_a94112d82bfd22477a1e892574876ec14132cf5552922847ef2001faa6708b5a_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_a94112d82bfd22477a1e892574876ec14132cf5552922847ef2001faa6708b5a_, "js");
        echo "',
    '";
        // line 37
        ob_start(function () { return ''; });
        // l10n: Short month name for April
        echo _gettext("Apr");
        $___internal_a1ff38a2b4c6730d73c005beaa761cb2fbea37c0a1f91924a0393cbf14a95815_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_a1ff38a2b4c6730d73c005beaa761cb2fbea37c0a1f91924a0393cbf14a95815_, "js");
        echo "',
    '";
        // line 38
        ob_start(function () { return ''; });
        // l10n: Short month name for May
        echo _gettext("May");
        $___internal_06d8b9de2bab926b3fb28a1ba5793bd85295a7b430944394f2c7987d57ca6baf_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_06d8b9de2bab926b3fb28a1ba5793bd85295a7b430944394f2c7987d57ca6baf_, "js");
        echo "',
    '";
        // line 39
        ob_start(function () { return ''; });
        // l10n: Short month name for June
        echo _gettext("Jun");
        $___internal_84832aa48fb391579a7898089315c55bdd8197a803a1d3526f95a1d592e987e7_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_84832aa48fb391579a7898089315c55bdd8197a803a1d3526f95a1d592e987e7_, "js");
        echo "',
    '";
        // line 40
        ob_start(function () { return ''; });
        // l10n: Short month name for July
        echo _gettext("Jul");
        $___internal_cdc92e6cc0d0c7065e07ada418f27c075347ed4f9e8bce28ef52b3d046eca94d_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_cdc92e6cc0d0c7065e07ada418f27c075347ed4f9e8bce28ef52b3d046eca94d_, "js");
        echo "',
    '";
        // line 41
        ob_start(function () { return ''; });
        // l10n: Short month name for August
        echo _gettext("Aug");
        $___internal_74b9e61b7028e9daf167bdfeeb87096041cf502ee7e7abf5effa2ecb3508284d_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_74b9e61b7028e9daf167bdfeeb87096041cf502ee7e7abf5effa2ecb3508284d_, "js");
        echo "',
    '";
        // line 42
        ob_start(function () { return ''; });
        // l10n: Short month name for September
        echo _gettext("Sep");
        $___internal_7f673a6ffcab8e4733938fc2da765e53dde039b73363168d695986e1ae7bbe66_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_7f673a6ffcab8e4733938fc2da765e53dde039b73363168d695986e1ae7bbe66_, "js");
        echo "',
    '";
        // line 43
        ob_start(function () { return ''; });
        // l10n: Short month name for October
        echo _gettext("Oct");
        $___internal_79b571770d866b4ee28e907049b5e25dc6b791cc1613a23023f43b421810dd31_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_79b571770d866b4ee28e907049b5e25dc6b791cc1613a23023f43b421810dd31_, "js");
        echo "',
    '";
        // line 44
        ob_start(function () { return ''; });
        // l10n: Short month name for November
        echo _gettext("Nov");
        $___internal_35f7cfcbdfd1aab3fdf8d70e04abb57c401ce9546d11079e816918deea68efab_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_35f7cfcbdfd1aab3fdf8d70e04abb57c401ce9546d11079e816918deea68efab_, "js");
        echo "',
    '";
        // line 45
        ob_start(function () { return ''; });
        // l10n: Short month name for December
        echo _gettext("Dec");
        $___internal_7d53948b70531b0e47da23686b561f655e7a8cf369211e3d7f2e3b5a1d13f228_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_7d53948b70531b0e47da23686b561f655e7a8cf369211e3d7f2e3b5a1d13f228_, "js");
        echo "',
  ];
  \$.datepicker.regional[''].dayNames = [
    '";
        // line 48
        echo twig_escape_filter($this->env, _gettext("Sunday"), "js", null, true);
        echo "',
    '";
        // line 49
        echo twig_escape_filter($this->env, _gettext("Monday"), "js", null, true);
        echo "',
    '";
        // line 50
        echo twig_escape_filter($this->env, _gettext("Tuesday"), "js", null, true);
        echo "',
    '";
        // line 51
        echo twig_escape_filter($this->env, _gettext("Wednesday"), "js", null, true);
        echo "',
    '";
        // line 52
        echo twig_escape_filter($this->env, _gettext("Thursday"), "js", null, true);
        echo "',
    '";
        // line 53
        echo twig_escape_filter($this->env, _gettext("Friday"), "js", null, true);
        echo "',
    '";
        // line 54
        echo twig_escape_filter($this->env, _gettext("Saturday"), "js", null, true);
        echo "',
  ];
  \$.datepicker.regional[''].dayNamesShort = [
    '";
        // line 57
        ob_start(function () { return ''; });
        // l10n: Short week day name for Sunday
        echo _gettext("Sun");
        $___internal_a345fdc04523e65af89928dd012051ff994f543b116a724278f9de0b302cbcfd_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_a345fdc04523e65af89928dd012051ff994f543b116a724278f9de0b302cbcfd_, "js");
        echo "',
    '";
        // line 58
        ob_start(function () { return ''; });
        // l10n: Short week day name for Monday
        echo _gettext("Mon");
        $___internal_d48153894f7ef87a6eac1cd7e82a83d39563f858231b1d56390493d281b756d7_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_d48153894f7ef87a6eac1cd7e82a83d39563f858231b1d56390493d281b756d7_, "js");
        echo "',
    '";
        // line 59
        ob_start(function () { return ''; });
        // l10n: Short week day name for Tuesday
        echo _gettext("Tue");
        $___internal_b64c6653073cea10d6ee91bebc24c06bdc8ca2501da62dbad78825c2c7b9d7c4_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_b64c6653073cea10d6ee91bebc24c06bdc8ca2501da62dbad78825c2c7b9d7c4_, "js");
        echo "',
    '";
        // line 60
        ob_start(function () { return ''; });
        // l10n: Short week day name for Wednesday
        echo _gettext("Wed");
        $___internal_7db174e912ea01140178f423117fd44b97634bc68e287b2a2504f34b5e85e38c_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_7db174e912ea01140178f423117fd44b97634bc68e287b2a2504f34b5e85e38c_, "js");
        echo "',
    '";
        // line 61
        ob_start(function () { return ''; });
        // l10n: Short week day name for Thursday
        echo _gettext("Thu");
        $___internal_7514d53480d42251b0a7e5836a3b2c86a3cfe365da407199ae314b093e372a66_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_7514d53480d42251b0a7e5836a3b2c86a3cfe365da407199ae314b093e372a66_, "js");
        echo "',
    '";
        // line 62
        ob_start(function () { return ''; });
        // l10n: Short week day name for Friday
        echo _gettext("Fri");
        $___internal_dc3647a1e03d667439d6f8224bc54ab171f53e36e59af2ecd697d33f50163457_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_dc3647a1e03d667439d6f8224bc54ab171f53e36e59af2ecd697d33f50163457_, "js");
        echo "',
    '";
        // line 63
        ob_start(function () { return ''; });
        // l10n: Short week day name for Saturday
        echo _gettext("Sat");
        $___internal_e14a1916535d7bd3ea9bd3de5ff3cd371bdb03b72c8d95d09b18c0bb2a5a64e6_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_e14a1916535d7bd3ea9bd3de5ff3cd371bdb03b72c8d95d09b18c0bb2a5a64e6_, "js");
        echo "',
  ];
  \$.datepicker.regional[''].dayNamesMin = [
    '";
        // line 66
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Sunday
        echo _gettext("Su");
        $___internal_6b8339b591132ed8900ac7b3c01b59a86e36edd7b06cc1640b09677719e40535_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_6b8339b591132ed8900ac7b3c01b59a86e36edd7b06cc1640b09677719e40535_, "js");
        echo "',
    '";
        // line 67
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Monday
        echo _gettext("Mo");
        $___internal_47282adfcf091fe90b8779a97f647c96f60b980f6426db098f733b332b2d1445_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_47282adfcf091fe90b8779a97f647c96f60b980f6426db098f733b332b2d1445_, "js");
        echo "',
    '";
        // line 68
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Tuesday
        echo _gettext("Tu");
        $___internal_215bcb1f2986368900ebc25f86f8d4ef47ffdfcd68822644e087d239e88f821e_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_215bcb1f2986368900ebc25f86f8d4ef47ffdfcd68822644e087d239e88f821e_, "js");
        echo "',
    '";
        // line 69
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Wednesday
        echo _gettext("We");
        $___internal_61bccc634fdc46ef535eef27299a141b724a88e1554382a0ff5bd8abe24a3374_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_61bccc634fdc46ef535eef27299a141b724a88e1554382a0ff5bd8abe24a3374_, "js");
        echo "',
    '";
        // line 70
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Thursday
        echo _gettext("Th");
        $___internal_4a8dd75a10adfd68336238f040c38b7fab786c890489e1987b51b948ec85f080_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_4a8dd75a10adfd68336238f040c38b7fab786c890489e1987b51b948ec85f080_, "js");
        echo "',
    '";
        // line 71
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Friday
        echo _gettext("Fr");
        $___internal_fcc0566048126eeb601c89b8d5ca8459c54a99c65c676b77cd6dcc2b935e0a7d_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_fcc0566048126eeb601c89b8d5ca8459c54a99c65c676b77cd6dcc2b935e0a7d_, "js");
        echo "',
    '";
        // line 72
        ob_start(function () { return ''; });
        // l10n: Minimal week day name for Saturday
        echo _gettext("Sa");
        $___internal_0470d5dd388f929feb6e08a1b246a297a8b8d26879bb2ec374f06c95541c3024_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_0470d5dd388f929feb6e08a1b246a297a8b8d26879bb2ec374f06c95541c3024_, "js");
        echo "',
  ];
  \$.datepicker.regional[''].weekHeader = '";
        // line 74
        ob_start(function () { return ''; });
        // l10n: Column header for week of the year in calendar
        echo _gettext("Wk");
        $___internal_bdf9af82f53cab62e2fc57ef1afcbf957aabbf247c7bf3a5273ae9951dd34743_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
        echo twig_escape_filter($this->env, $___internal_bdf9af82f53cab62e2fc57ef1afcbf957aabbf247c7bf3a5273ae9951dd34743_, "js");
        echo "';
  \$.datepicker.regional[''].showMonthAfterYear = ";
        // line 75
        echo (((($context["show_month_after_year"] ?? null) == "calendar-year-month")) ? ("true") : ("false"));
        echo ";
  \$.datepicker.regional[''].yearSuffix = '";
        // line 76
        echo (((($context["year_suffix"] ?? null) != "none")) ? (twig_escape_filter($this->env, ($context["year_suffix"] ?? null), "js")) : (""));
        echo "';
  \$.extend(\$.datepicker._defaults, \$.datepicker.regional['']);
}

if (\$.timepicker) {
  \$.timepicker.regional[''].timeText = '";
        // line 81
        echo twig_escape_filter($this->env, _gettext("Time"), "js", null, true);
        echo "';
  \$.timepicker.regional[''].hourText = '";
        // line 82
        echo twig_escape_filter($this->env, _gettext("Hour"), "js", null, true);
        echo "';
  \$.timepicker.regional[''].minuteText = '";
        // line 83
        echo twig_escape_filter($this->env, _gettext("Minute"), "js", null, true);
        echo "';
  \$.timepicker.regional[''].secondText = '";
        // line 84
        echo twig_escape_filter($this->env, _gettext("Second"), "js", null, true);
        echo "';
  \$.extend(\$.timepicker._defaults, \$.timepicker.regional['']);
}

function extendingValidatorMessages () {
  \$.extend(\$.validator.messages, {
    required: '";
        // line 90
        echo twig_escape_filter($this->env, _gettext("This field is required"), "js", null, true);
        echo "',
    remote: '";
        // line 91
        echo twig_escape_filter($this->env, _gettext("Please fix this field"), "js", null, true);
        echo "',
    email: '";
        // line 92
        echo twig_escape_filter($this->env, _gettext("Please enter a valid email address"), "js", null, true);
        echo "',
    url: '";
        // line 93
        echo twig_escape_filter($this->env, _gettext("Please enter a valid URL"), "js", null, true);
        echo "',
    date: '";
        // line 94
        echo twig_escape_filter($this->env, _gettext("Please enter a valid date"), "js", null, true);
        echo "',
    dateISO: '";
        // line 95
        echo twig_escape_filter($this->env, _gettext("Please enter a valid date ( ISO )"), "js", null, true);
        echo "',
    number: '";
        // line 96
        echo twig_escape_filter($this->env, _gettext("Please enter a valid number"), "js", null, true);
        echo "',
    creditcard: '";
        // line 97
        echo twig_escape_filter($this->env, _gettext("Please enter a valid credit card number"), "js", null, true);
        echo "',
    digits: '";
        // line 98
        echo twig_escape_filter($this->env, _gettext("Please enter only digits"), "js", null, true);
        echo "',
    equalTo: '";
        // line 99
        echo twig_escape_filter($this->env, _gettext("Please enter the same value again"), "js", null, true);
        echo "',
    maxlength: \$.validator.format('";
        // line 100
        echo twig_escape_filter($this->env, _gettext("Please enter no more than {0} characters"), "js", null, true);
        echo "'),
    minlength: \$.validator.format('";
        // line 101
        echo twig_escape_filter($this->env, _gettext("Please enter at least {0} characters"), "js", null, true);
        echo "'),
    rangelength: \$.validator.format('";
        // line 102
        echo twig_escape_filter($this->env, _gettext("Please enter a value between {0} and {1} characters long"), "js", null, true);
        echo "'),
    range: \$.validator.format('";
        // line 103
        echo twig_escape_filter($this->env, _gettext("Please enter a value between {0} and {1}"), "js", null, true);
        echo "'),
    max: \$.validator.format('";
        // line 104
        echo twig_escape_filter($this->env, _gettext("Please enter a value less than or equal to {0}"), "js", null, true);
        echo "'),
    min: \$.validator.format('";
        // line 105
        echo twig_escape_filter($this->env, _gettext("Please enter a value greater than or equal to {0}"), "js", null, true);
        echo "'),
    validationFunctionForDateTime: \$.validator.format('";
        // line 106
        echo twig_escape_filter($this->env, _gettext("Please enter a valid date or time"), "js", null, true);
        echo "'),
    validationFunctionForHex: \$.validator.format('";
        // line 107
        echo twig_escape_filter($this->env, _gettext("Please enter a valid HEX input"), "js", null, true);
        echo "'),
    validationFunctionForFuns: \$.validator.format('";
        // line 108
        echo twig_escape_filter($this->env, _gettext("Error"), "js", null, true);
        echo "')
  });
}
";
    }

    public function getTemplateName()
    {
        return "javascript/variables.twig";
    }

    public function isTraitable()
    {
        return false;
    }

    public function getDebugInfo()
    {
        return array (  509 => 108,  505 => 107,  501 => 106,  497 => 105,  493 => 104,  489 => 103,  485 => 102,  481 => 101,  477 => 100,  473 => 99,  469 => 98,  465 => 97,  461 => 96,  457 => 95,  453 => 94,  449 => 93,  445 => 92,  441 => 91,  437 => 90,  428 => 84,  424 => 83,  420 => 82,  416 => 81,  408 => 76,  404 => 75,  396 => 74,  387 => 72,  379 => 71,  371 => 70,  363 => 69,  355 => 68,  347 => 67,  339 => 66,  329 => 63,  321 => 62,  313 => 61,  305 => 60,  297 => 59,  289 => 58,  281 => 57,  275 => 54,  271 => 53,  267 => 52,  263 => 51,  259 => 50,  255 => 49,  251 => 48,  241 => 45,  233 => 44,  225 => 43,  217 => 42,  209 => 41,  201 => 40,  193 => 39,  185 => 38,  177 => 37,  169 => 36,  161 => 35,  153 => 34,  147 => 31,  143 => 30,  139 => 29,  135 => 28,  131 => 27,  127 => 26,  123 => 25,  119 => 24,  115 => 23,  111 => 22,  107 => 21,  103 => 20,  94 => 18,  86 => 17,  78 => 16,  70 => 15,  67 => 14,  63 => 11,  61 => 10,  57 => 8,  55 => 7,  50 => 5,  46 => 4,  42 => 3,  37 => 2,);
    }

    public function getSourceContext()
    {
        return new Source("", "javascript/variables.twig", "C:\\OpenServer\\modules\\system\\html\\openserver\\phpmyadmin\\templates\\javascript\\variables.twig");
    }
}
