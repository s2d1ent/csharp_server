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

/* home/index.twig */
class __TwigTemplate_2a4692c5b1b9d8130024d0cda409e80ba60faff636bafad7061515628e62abaa extends \Twig\Template
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
        // line 1
        if (($context["is_git_revision"] ?? null)) {
            // line 2
            echo "  <div id=\"is_git_revision\"></div>
";
        }
        // line 4
        echo "
";
        // line 5
        echo ($context["message"] ?? null);
        echo "

";
        // line 7
        echo ($context["partial_logout"] ?? null);
        echo "

<div id=\"maincontainer\">
  ";
        // line 10
        echo ($context["sync_favorite_tables"] ?? null);
        echo "
  <div class=\"container-fluid\">
    <div class=\"row\">
      <div class=\"col-lg-7 col-12\">
        ";
        // line 14
        if (($context["has_server"] ?? null)) {
            // line 15
            echo "          ";
            if (($context["is_demo"] ?? null)) {
                // line 16
                echo "            <div class=\"card mt-4\">
              <div class=\"card-header\">
                ";
                // line 18
                echo _gettext("phpMyAdmin Demo Server");
                // line 19
                echo "              </div>
              <div class=\"card-body\">
                ";
                // line 21
                ob_start(function () { return ''; });
                // line 22
                echo "                  ";
                echo _gettext("You are using the demo server. You can do anything here, but please do not change root, debian-sys-maint and pma users. More information is available at %s.");
                // line 25
                echo "                ";
                $___internal_b54be7dd759365acf2c0aff36bd6a60d0bdbb9a895f867add2bf115f07c4699c_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
                // line 21
                echo sprintf($___internal_b54be7dd759365acf2c0aff36bd6a60d0bdbb9a895f867add2bf115f07c4699c_, "<a href=\"url.php?url=https://demo.phpmyadmin.net/\" target=\"_blank\" rel=\"noopener noreferrer\">demo.phpmyadmin.net</a>");
                // line 26
                echo "              </div>
            </div>
          ";
            }
            // line 29
            echo "
            <div class=\"card mt-4\">
              <div class=\"card-header\">
                ";
            // line 32
            echo _gettext("General settings");
            // line 33
            echo "              </div>
              <ul class=\"list-group list-group-flush\">
                ";
            // line 35
            if (($context["has_server_selection"] ?? null)) {
                // line 36
                echo "                  <li id=\"li_select_server\" class=\"list-group-item\">
                    ";
                // line 37
                echo \PhpMyAdmin\Html\Generator::getImage("s_host");
                echo "
                    ";
                // line 38
                echo ($context["server_selection"] ?? null);
                echo "
                  </li>
                ";
            }
            // line 41
            echo "
                ";
            // line 42
            if ((($context["server"] ?? null) > 0)) {
                // line 43
                echo "                  ";
                if (($context["has_change_password_link"] ?? null)) {
                    // line 44
                    echo "                    <li id=\"li_change_password\" class=\"list-group-item\">
                      <a href=\"";
                    // line 45
                    echo PhpMyAdmin\Url::getFromRoute("/user-password");
                    echo "\" id=\"change_password_anchor\" class=\"ajax\">
                        ";
                    // line 46
                    echo \PhpMyAdmin\Html\Generator::getIcon("s_passwd", _gettext("Change password"), true);
                    echo "
                      </a>
                    </li>
                  ";
                }
                // line 50
                echo "
                  <li id=\"li_select_mysql_collation\" class=\"list-group-item\">
                    <form class=\"disableAjax\" method=\"post\" action=\"";
                // line 52
                echo PhpMyAdmin\Url::getFromRoute("/collation-connection");
                echo "\">
                      ";
                // line 53
                echo PhpMyAdmin\Url::getHiddenInputs(null, null, 4, "collation_connection");
                echo "
                      <label for=\"select_collation_connection\">
                        ";
                // line 55
                echo \PhpMyAdmin\Html\Generator::getImage("s_asci");
                echo "
                        ";
                // line 56
                echo _gettext("Server connection collation:");
                // line 57
                echo "                        ";
                echo \PhpMyAdmin\Html\MySQLDocumentation::show("charset-connection");
                echo "
                      </label>
                      ";
                // line 59
                if ( !twig_test_empty(($context["charsets"] ?? null))) {
                    // line 60
                    echo "                        <select lang=\"en\" dir=\"ltr\" name=\"collation_connection\" id=\"select_collation_connection\" class=\"autosubmit\">
                          <option value=\"\">";
                    // line 61
                    echo _gettext("Collation");
                    echo "</option>
                          <option value=\"\"></option>
                          ";
                    // line 63
                    $context['_parent'] = $context;
                    $context['_seq'] = twig_ensure_traversable(($context["charsets"] ?? null));
                    foreach ($context['_seq'] as $context["_key"] => $context["charset"]) {
                        // line 64
                        echo "                            <optgroup label=\"";
                        echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, $context["charset"], "name", [], "any", false, false, false, 64), "html", null, true);
                        echo "\" title=\"";
                        echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, $context["charset"], "description", [], "any", false, false, false, 64), "html", null, true);
                        echo "\">
                              ";
                        // line 65
                        $context['_parent'] = $context;
                        $context['_seq'] = twig_ensure_traversable(twig_get_attribute($this->env, $this->source, $context["charset"], "collations", [], "any", false, false, false, 65));
                        foreach ($context['_seq'] as $context["_key"] => $context["collation"]) {
                            // line 66
                            echo "                                <option value=\"";
                            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, $context["collation"], "name", [], "any", false, false, false, 66), "html", null, true);
                            echo "\" title=\"";
                            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, $context["collation"], "description", [], "any", false, false, false, 66), "html", null, true);
                            echo "\"";
                            echo ((twig_get_attribute($this->env, $this->source, $context["collation"], "is_selected", [], "any", false, false, false, 66)) ? (" selected") : (""));
                            echo ">";
                            // line 67
                            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, $context["collation"], "name", [], "any", false, false, false, 67), "html", null, true);
                            // line 68
                            echo "</option>
                              ";
                        }
                        $_parent = $context['_parent'];
                        unset($context['_seq'], $context['_iterated'], $context['_key'], $context['collation'], $context['_parent'], $context['loop']);
                        $context = array_intersect_key($context, $_parent) + $_parent;
                        // line 70
                        echo "                            </optgroup>
                          ";
                    }
                    $_parent = $context['_parent'];
                    unset($context['_seq'], $context['_iterated'], $context['_key'], $context['charset'], $context['_parent'], $context['loop']);
                    $context = array_intersect_key($context, $_parent) + $_parent;
                    // line 72
                    echo "                        </select>
                      ";
                }
                // line 74
                echo "                    </form>
                  </li>

                  <li id=\"li_user_preferences\" class=\"list-group-item\">
                    <a href=\"";
                // line 78
                echo PhpMyAdmin\Url::getFromRoute("/preferences/manage");
                echo "\">
                      ";
                // line 79
                echo \PhpMyAdmin\Html\Generator::getIcon("b_tblops", _gettext("More settings"), true);
                echo "
                    </a>
                  </li>
                ";
            }
            // line 83
            echo "              </ul>
            </div>
          ";
        }
        // line 86
        echo "
            <div class=\"card mt-4\">
              <div class=\"card-header\">
                ";
        // line 89
        echo _gettext("Appearance settings");
        // line 90
        echo "              </div>
              <ul class=\"list-group list-group-flush\">
                ";
        // line 92
        if ( !twig_test_empty(($context["language_selector"] ?? null))) {
            // line 93
            echo "                  <li id=\"li_select_lang\" class=\"list-group-item\">
                    ";
            // line 94
            echo \PhpMyAdmin\Html\Generator::getImage("s_lang");
            echo "
                    ";
            // line 95
            echo ($context["language_selector"] ?? null);
            echo "
                  </li>
                ";
        }
        // line 98
        echo "
                ";
        // line 99
        if ( !twig_test_empty(($context["theme_selection"] ?? null))) {
            // line 100
            echo "                  <li id=\"li_select_theme\" class=\"list-group-item\">
                    ";
            // line 101
            echo \PhpMyAdmin\Html\Generator::getImage("s_theme");
            echo "
                    ";
            // line 102
            echo ($context["theme_selection"] ?? null);
            echo "
                  </li>
                ";
        }
        // line 105
        echo "              </ul>
            </div>
          </div>

      <div class=\"col-lg-5 col-12\">
        ";
        // line 110
        if ( !twig_test_empty(($context["database_server"] ?? null))) {
            // line 111
            echo "          <div class=\"card mt-4\">
            <div class=\"card-header\">
              ";
            // line 113
            echo _gettext("Database server");
            // line 114
            echo "            </div>
            <ul class=\"list-group list-group-flush\">
              <li class=\"list-group-item\">
                ";
            // line 117
            echo _gettext("Server:");
            // line 118
            echo "                ";
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "host", [], "any", false, false, false, 118), "html", null, true);
            echo "
              </li>
              <li class=\"list-group-item\">
                ";
            // line 121
            echo _gettext("Server type:");
            // line 122
            echo "                ";
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "type", [], "any", false, false, false, 122), "html", null, true);
            echo "
              </li>
              <li class=\"list-group-item\">
                ";
            // line 125
            echo _gettext("Server connection:");
            // line 126
            echo "                ";
            echo twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "connection", [], "any", false, false, false, 126);
            echo "
              </li>
              <li class=\"list-group-item\">
                ";
            // line 129
            echo _gettext("Server version:");
            // line 130
            echo "                ";
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "version", [], "any", false, false, false, 130), "html", null, true);
            echo "
              </li>
              <li class=\"list-group-item\">
                ";
            // line 133
            echo _gettext("Protocol version:");
            // line 134
            echo "                ";
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "protocol", [], "any", false, false, false, 134), "html", null, true);
            echo "
              </li>
              <li class=\"list-group-item\">
                ";
            // line 137
            echo _gettext("User:");
            // line 138
            echo "                ";
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "user", [], "any", false, false, false, 138), "html", null, true);
            echo "
              </li>
              <li class=\"list-group-item\">
                ";
            // line 141
            echo _gettext("Server charset:");
            // line 142
            echo "                <span lang=\"en\" dir=\"ltr\">
                  ";
            // line 143
            echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["database_server"] ?? null), "charset", [], "any", false, false, false, 143), "html", null, true);
            echo "
                </span>
              </li>
            </ul>
          </div>
        ";
        }
        // line 149
        echo "
        ";
        // line 150
        if (( !twig_test_empty(($context["web_server"] ?? null)) || ($context["show_php_info"] ?? null))) {
            // line 151
            echo "          <div class=\"card mt-4\">
            <div class=\"card-header\">
              ";
            // line 153
            echo _gettext("Web server");
            // line 154
            echo "            </div>
            <ul class=\"list-group list-group-flush\">
              ";
            // line 156
            if ( !twig_test_empty(($context["web_server"] ?? null))) {
                // line 157
                echo "                <li class=\"list-group-item\">
                  ";
                // line 158
                echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["web_server"] ?? null), "software", [], "any", false, false, false, 158), "html", null, true);
                echo "
                </li>
                <li class=\"list-group-item\" id=\"li_mysql_client_version\">
                  ";
                // line 161
                echo _gettext("Database client version:");
                // line 162
                echo "                  ";
                echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["web_server"] ?? null), "database", [], "any", false, false, false, 162), "html", null, true);
                echo "
                </li>
                <li class=\"list-group-item\">
                  ";
                // line 165
                echo _gettext("PHP extension:");
                // line 166
                echo "                  ";
                $context['_parent'] = $context;
                $context['_seq'] = twig_ensure_traversable(twig_get_attribute($this->env, $this->source, ($context["web_server"] ?? null), "php_extensions", [], "any", false, false, false, 166));
                foreach ($context['_seq'] as $context["_key"] => $context["extension"]) {
                    // line 167
                    echo "                    ";
                    echo twig_escape_filter($this->env, $context["extension"], "html", null, true);
                    echo "
                    ";
                    // line 168
                    echo \PhpMyAdmin\Html\Generator::showPHPDocumentation((("book." . $context["extension"]) . ".php"));
                    echo "
                  ";
                }
                $_parent = $context['_parent'];
                unset($context['_seq'], $context['_iterated'], $context['_key'], $context['extension'], $context['_parent'], $context['loop']);
                $context = array_intersect_key($context, $_parent) + $_parent;
                // line 170
                echo "                </li>
                <li class=\"list-group-item\">
                  ";
                // line 172
                echo _gettext("PHP version:");
                // line 173
                echo "                  ";
                echo twig_escape_filter($this->env, twig_get_attribute($this->env, $this->source, ($context["web_server"] ?? null), "php_version", [], "any", false, false, false, 173), "html", null, true);
                echo "
                </li>
              ";
            }
            // line 176
            echo "              ";
            if (($context["show_php_info"] ?? null)) {
                // line 177
                echo "                <li class=\"list-group-item\">
                  <a href=\"";
                // line 178
                echo PhpMyAdmin\Url::getFromRoute("/phpinfo");
                echo "\" target=\"_blank\" rel=\"noopener noreferrer\">
                    ";
                // line 179
                echo _gettext("Show PHP information");
                // line 180
                echo "                  </a>
                </li>
              ";
            }
            // line 183
            echo "            </ul>
          </div>
        ";
        }
        // line 186
        echo "
          <div class=\"card mt-4\">
            <div class=\"card-header\">
              phpMyAdmin
            </div>
            <ul class=\"list-group list-group-flush\">
              <li id=\"li_pma_version\" class=\"list-group-item";
        // line 192
        echo ((($context["is_version_checked"] ?? null)) ? (" jsversioncheck") : (""));
        echo "\">
                ";
        // line 193
        echo _gettext("Version information:");
        // line 194
        echo "                <span class=\"version\">";
        echo twig_escape_filter($this->env, ($context["phpmyadmin_version"] ?? null), "html", null, true);
        echo "</span>
              </li>
              <li class=\"list-group-item\">
                <a href=\"";
        // line 197
        echo \PhpMyAdmin\Html\MySQLDocumentation::getDocumentationLink("index");
        echo "\" target=\"_blank\" rel=\"noopener noreferrer\">
                  ";
        // line 198
        echo _gettext("Documentation");
        // line 199
        echo "                </a>
              </li>
              <li class=\"list-group-item\">
                <a href=\"";
        // line 202
        echo twig_escape_filter($this->env, PhpMyAdmin\Core::linkURL("https://www.phpmyadmin.net/"), "html", null, true);
        echo "\" target=\"_blank\" rel=\"noopener noreferrer\">
                  ";
        // line 203
        echo _gettext("Official Homepage");
        // line 204
        echo "                </a>
              </li>
              <li class=\"list-group-item\">
                <a href=\"";
        // line 207
        echo twig_escape_filter($this->env, PhpMyAdmin\Core::linkURL("https://www.phpmyadmin.net/contribute/"), "html", null, true);
        echo "\" target=\"_blank\" rel=\"noopener noreferrer\">
                  ";
        // line 208
        echo _gettext("Contribute");
        // line 209
        echo "                </a>
              </li>
              <li class=\"list-group-item\">
                <a href=\"";
        // line 212
        echo twig_escape_filter($this->env, PhpMyAdmin\Core::linkURL("https://www.phpmyadmin.net/support/"), "html", null, true);
        echo "\" target=\"_blank\" rel=\"noopener noreferrer\">
                  ";
        // line 213
        echo _gettext("Get support");
        // line 214
        echo "                </a>
              </li>
              <li class=\"list-group-item\">
                <a href=\"";
        // line 217
        echo PhpMyAdmin\Url::getFromRoute("/changelog");
        echo "\" target=\"_blank\">
                  ";
        // line 218
        echo _gettext("List of changes");
        // line 219
        echo "                </a>
              </li>
              <li class=\"list-group-item\">
                <a href=\"";
        // line 222
        echo PhpMyAdmin\Url::getFromRoute("/license");
        echo "\" target=\"_blank\">
                  ";
        // line 223
        echo _gettext("License");
        // line 224
        echo "                </a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>

";
        // line 233
        echo ($context["config_storage_message"] ?? null);
        echo "
";
    }

    public function getTemplateName()
    {
        return "home/index.twig";
    }

    public function isTraitable()
    {
        return false;
    }

    public function getDebugInfo()
    {
        return array (  557 => 233,  546 => 224,  544 => 223,  540 => 222,  535 => 219,  533 => 218,  529 => 217,  524 => 214,  522 => 213,  518 => 212,  513 => 209,  511 => 208,  507 => 207,  502 => 204,  500 => 203,  496 => 202,  491 => 199,  489 => 198,  485 => 197,  478 => 194,  476 => 193,  472 => 192,  464 => 186,  459 => 183,  454 => 180,  452 => 179,  448 => 178,  445 => 177,  442 => 176,  435 => 173,  433 => 172,  429 => 170,  421 => 168,  416 => 167,  411 => 166,  409 => 165,  402 => 162,  400 => 161,  394 => 158,  391 => 157,  389 => 156,  385 => 154,  383 => 153,  379 => 151,  377 => 150,  374 => 149,  365 => 143,  362 => 142,  360 => 141,  353 => 138,  351 => 137,  344 => 134,  342 => 133,  335 => 130,  333 => 129,  326 => 126,  324 => 125,  317 => 122,  315 => 121,  308 => 118,  306 => 117,  301 => 114,  299 => 113,  295 => 111,  293 => 110,  286 => 105,  280 => 102,  276 => 101,  273 => 100,  271 => 99,  268 => 98,  262 => 95,  258 => 94,  255 => 93,  253 => 92,  249 => 90,  247 => 89,  242 => 86,  237 => 83,  230 => 79,  226 => 78,  220 => 74,  216 => 72,  209 => 70,  202 => 68,  200 => 67,  192 => 66,  188 => 65,  181 => 64,  177 => 63,  172 => 61,  169 => 60,  167 => 59,  161 => 57,  159 => 56,  155 => 55,  150 => 53,  146 => 52,  142 => 50,  135 => 46,  131 => 45,  128 => 44,  125 => 43,  123 => 42,  120 => 41,  114 => 38,  110 => 37,  107 => 36,  105 => 35,  101 => 33,  99 => 32,  94 => 29,  89 => 26,  87 => 21,  84 => 25,  81 => 22,  79 => 21,  75 => 19,  73 => 18,  69 => 16,  66 => 15,  64 => 14,  57 => 10,  51 => 7,  46 => 5,  43 => 4,  39 => 2,  37 => 1,);
    }

    public function getSourceContext()
    {
        return new Source("", "home/index.twig", "C:\\OpenServer\\modules\\system\\html\\openserver\\phpmyadmin\\templates\\home\\index.twig");
    }
}
