-- MySQL dump 10.13  Distrib 5.1.41, for Win32 (ia32)
--
-- Host: localhost    Database: net_rmxp_project
-- ------------------------------------------------------
-- Server version	5.1.41-community

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `enemy`
--

DROP TABLE IF EXISTS `enemy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `enemy` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `name` char(255) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `image` char(255) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `exp` int(10) NOT NULL DEFAULT '0',
  `level` int(10) NOT NULL DEFAULT '1',
  `maxhp` int(10) NOT NULL DEFAULT '100',
  `maxmp` int(10) NOT NULL DEFAULT '100',
  `str` int(10) NOT NULL DEFAULT '5',
  `dex` int(10) NOT NULL DEFAULT '5',
  `int` int(10) NOT NULL DEFAULT '5',
  `luk` int(10) NOT NULL DEFAULT '5',
  `direction` int(10) NOT NULL DEFAULT '2',
  `move_speed` int(10) NOT NULL DEFAULT '4',
  `pattern` int(10) NOT NULL DEFAULT '0',
  `delay` int(10) NOT NULL DEFAULT '10',
  `rebirth_time` int(10) NOT NULL DEFAULT '50',
  `sight` int(10) NOT NULL DEFAULT '3',
  `animation_id` int(10) NOT NULL DEFAULT '7',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=47 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `enemy`
--

LOCK TABLES `enemy` WRITE;
/*!40000 ALTER TABLE `enemy` DISABLE KEYS */;
INSERT INTO `enemy` VALUES (37,'다람쥐','168-Small10.png',5,1,100,100,8,20,5,3,2,4,0,10,50,1,4),(45,'독버섯','090-Monster04.png',130,15,2500,100,40,140,5,100,2,5,0,7,50,3,9),(39,'들개','151-Animal01.png',25,3,350,100,19,40,5,5,2,4,0,10,50,3,6),(44,'[보스] 호랑이','158-Animal08.png',550,10,5000,100,200,145,5,150,2,7,0,4,600,3,6),(46,'Shadow Snow','062-Aquatic04',99999,50,2000,100,5,5,5,5,2,4,0,10,120,3,5);
/*!40000 ALTER TABLE `enemy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `enemy_dropitem`
--

DROP TABLE IF EXISTS `enemy_dropitem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `enemy_dropitem` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `enemy_no` int(10) NOT NULL,
  `item_no` int(10) NOT NULL,
  `rate` int(10) NOT NULL DEFAULT '0',
  `image` char(255) COLLATE utf8_unicode_ci NOT NULL DEFAULT '193-Support01.png',
  `pattern_x` int(10) NOT NULL DEFAULT '0',
  `pattern_y` int(10) NOT NULL DEFAULT '2',
  `min_price` int(10) NOT NULL DEFAULT '0',
  `max_price` int(10) NOT NULL DEFAULT '0',
  `min_str` int(10) NOT NULL DEFAULT '0',
  `max_str` int(10) NOT NULL DEFAULT '0',
  `min_dex` int(10) NOT NULL DEFAULT '0',
  `max_dex` int(10) NOT NULL DEFAULT '0',
  `min_int` int(10) NOT NULL DEFAULT '0',
  `max_int` int(10) NOT NULL DEFAULT '0',
  `min_luk` int(10) NOT NULL DEFAULT '0',
  `max_luk` int(10) NOT NULL DEFAULT '0',
  `min_hp` int(10) NOT NULL DEFAULT '0',
  `max_hp` int(10) NOT NULL DEFAULT '0',
  `min_mp` int(10) NOT NULL DEFAULT '0',
  `max_mp` int(10) NOT NULL DEFAULT '0',
  `min_solid` int(10) NOT NULL DEFAULT '0',
  `max_solid` int(10) NOT NULL DEFAULT '0',
  `min_ability` int(10) NOT NULL DEFAULT '0',
  `max_ability` int(10) NOT NULL DEFAULT '0',
  `min_cost` int(10) NOT NULL DEFAULT '0',
  `max_cost` int(10) NOT NULL DEFAULT '0',
  `trade` int(10) NOT NULL DEFAULT '1',
  `sell` int(10) NOT NULL DEFAULT '1',
  `use` int(10) NOT NULL DEFAULT '1',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=48 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `enemy_dropitem`
--

LOCK TABLES `enemy_dropitem` WRITE;
/*!40000 ALTER TABLE `enemy_dropitem` DISABLE KEYS */;
INSERT INTO `enemy_dropitem` VALUES (38,42,60,100,'193-Support01.png',0,2,0,0,0,20,0,20,0,20,0,7,0,0,0,0,0,0,100,300,0,0,1,1,1),(37,41,60,100,'193-Support01.png',0,2,0,0,0,20,0,20,0,20,0,7,0,0,0,0,0,0,100,300,0,0,1,1,1),(36,40,60,100,'193-Support01.png',0,2,0,0,0,20,0,20,0,20,0,7,0,0,0,0,0,0,100,300,0,0,1,1,1),(35,39,61,10,'193-Support01.png',0,2,0,0,0,30,0,30,0,30,2,20,0,0,0,0,0,0,200,500,0,0,1,1,1),(34,39,60,100,'193-Support01.png',0,2,0,100,0,10,0,10,0,10,0,7,0,0,0,0,0,0,100,300,0,0,1,1,1),(33,38,59,100,'193-Support01.png',0,2,0,0,0,10,0,5,0,0,0,5,0,0,0,0,0,0,100,300,0,0,1,1,1),(32,36,59,100,'193-Support01.png',0,2,0,0,0,10,0,5,0,0,0,5,0,0,0,0,0,0,100,300,0,0,1,1,1),(31,35,59,100,'193-Support01.png',0,2,0,0,0,10,0,5,0,0,0,5,0,0,0,0,0,0,100,300,0,0,1,1,1),(26,37,59,100,'193-Support01.png',0,2,0,50,0,5,0,5,0,0,0,5,0,0,0,0,0,0,100,300,0,0,1,1,1),(24,38,58,300,'193-Support01.png',0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1),(22,37,58,300,'193-Support01.png',0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1),(23,36,58,300,'193-Support01.png',0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1),(21,35,58,300,'193-Support01.png',0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1),(40,40,61,10,'193-Support01.png',0,2,0,0,0,30,0,30,0,30,2,20,0,0,0,0,0,0,200,500,0,0,1,1,1),(39,43,60,100,'193-Support01.png',0,2,0,0,0,20,0,20,0,20,0,7,0,0,0,0,0,0,100,300,0,0,1,1,1),(41,41,61,10,'193-Support01.png',0,2,0,0,0,30,0,30,0,30,2,20,0,0,0,0,0,0,200,500,0,0,1,1,1),(42,42,61,10,'193-Support01.png',0,2,0,0,0,30,0,30,0,30,2,20,0,0,0,0,0,0,200,500,0,0,1,1,1),(43,43,61,10,'193-Support01.png',0,2,0,0,0,30,0,30,0,30,2,20,0,0,0,0,0,0,200,500,0,0,1,1,1),(44,44,62,300,'193-Support01.png',0,2,0,300,0,20,0,20,0,20,0,10,0,0,0,0,0,0,300,700,0,0,1,1,1),(45,44,63,200,'193-Support01.png',0,2,0,1000,0,50,0,50,0,50,0,25,0,0,0,0,0,0,500,1000,0,0,1,1,1),(46,45,67,400,'193-Support01.png',0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1),(47,44,68,150,'193-Support01.png',0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1);
/*!40000 ALTER TABLE `enemy_dropitem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `enemy_position`
--

DROP TABLE IF EXISTS `enemy_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `enemy_position` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `enemy_no` int(10) NOT NULL,
  `map_id` int(10) NOT NULL,
  `map_x` int(10) NOT NULL,
  `map_y` int(10) NOT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=20 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `enemy_position`
--

LOCK TABLES `enemy_position` WRITE;
/*!40000 ALTER TABLE `enemy_position` DISABLE KEYS */;
INSERT INTO `enemy_position` VALUES (1,37,2,11,2),(2,37,2,11,12),(3,37,2,22,14),(4,37,2,21,8),(5,39,3,9,2),(6,39,3,5,8),(7,39,3,11,15),(8,39,3,6,26),(9,39,3,14,3),(10,44,3,19,12),(11,45,4,12,2),(12,45,4,19,5),(13,45,4,20,13),(14,45,4,12,13),(15,45,4,4,14),(17,44,3,5,12),(18,44,3,5,27),(19,46,6,4,9);
/*!40000 ALTER TABLE `enemy_position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `farm_list`
--

DROP TABLE IF EXISTS `farm_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `farm_list` (
  `no` int(10) NOT NULL DEFAULT '0',
  `crop` char(10) COLLATE utf8_unicode_ci DEFAULT NULL,
  `num` int(10) DEFAULT NULL,
  `water` int(10) DEFAULT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `farm_list`
--

LOCK TABLES `farm_list` WRITE;
/*!40000 ALTER TABLE `farm_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `farm_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `level_up`
--

DROP TABLE IF EXISTS `level_up`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `level_up` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `next_exp` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=40 DEFAULT CHARSET=euckr;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `level_up`
--

LOCK TABLES `level_up` WRITE;
/*!40000 ALTER TABLE `level_up` DISABLE KEYS */;
INSERT INTO `level_up` VALUES (1,20),(2,40),(3,80),(4,120),(5,160),(6,200),(7,300),(8,400),(9,500),(10,600),(11,800),(12,1000),(13,1200),(14,1400),(15,1700),(16,2000),(17,2400),(18,2800),(19,3200),(20,6400),(21,9600),(22,12800),(23,25600),(24,51200),(25,102400),(26,204800),(27,409600),(28,819200),(29,1638400);
/*!40000 ALTER TABLE `level_up` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `npc`
--

DROP TABLE IF EXISTS `npc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `npc` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `name` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `image` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `id` int(10) NOT NULL DEFAULT '0',
  `map_id` int(10) NOT NULL DEFAULT '0',
  `map_x` int(10) NOT NULL DEFAULT '0',
  `map_y` int(10) NOT NULL DEFAULT '0',
  `direction` int(10) NOT NULL DEFAULT '2',
  `pattern` int(10) NOT NULL DEFAULT '0',
  `move_speed` int(10) NOT NULL DEFAULT '4',
  `default_action` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=14 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `npc`
--

LOCK TABLES `npc` WRITE;
/*!40000 ALTER TABLE `npc` DISABLE KEYS */;
INSERT INTO `npc` VALUES (10,'테스트NPC','020-Hunter01.png',1,1,12,7,2,0,4,1),(12,'어둠의 상인','018-Thief03.png',1,4,3,11,2,0,4,1),(13,'테스트상인','020-Hunter01.png',0,1,13,7,2,0,4,1);
/*!40000 ALTER TABLE `npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `portal`
--

DROP TABLE IF EXISTS `portal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `portal` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `map_id` int(10) NOT NULL,
  `x` int(10) NOT NULL,
  `y` int(10) NOT NULL,
  `move_map_id` int(10) NOT NULL,
  `move_x` int(10) NOT NULL,
  `move_y` int(10) NOT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=17 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `portal`
--

LOCK TABLES `portal` WRITE;
/*!40000 ALTER TABLE `portal` DISABLE KEYS */;
INSERT INTO `portal` VALUES (1,1,24,7,2,1,7),(2,1,24,8,2,1,8),(3,2,0,7,1,23,7),(4,2,0,8,1,23,8),(5,2,24,2,3,1,2),(6,2,24,3,3,1,3),(7,2,17,18,4,17,1),(8,2,18,18,4,18,1),(9,3,0,2,2,23,2),(10,3,0,3,2,23,3),(11,3,0,22,4,23,3),(12,3,0,23,4,23,4),(13,4,17,0,2,17,17),(14,4,18,0,2,18,17),(15,4,24,3,3,1,22),(16,4,24,4,3,1,23);
/*!40000 ALTER TABLE `portal` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_character`
--

DROP TABLE IF EXISTS `storage_character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_character` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `item_type` int(5) NOT NULL DEFAULT '2',
  `character` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `name` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `description` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `price` int(10) NOT NULL DEFAULT '0',
  `image` char(255) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_character`
--

LOCK TABLES `storage_character` WRITE;
/*!40000 ALTER TABLE `storage_character` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage_character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_equipment`
--

DROP TABLE IF EXISTS `storage_equipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_equipment` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `item_no` int(10) NOT NULL,
  `price` int(10) NOT NULL DEFAULT '0',
  `str` int(10) NOT NULL DEFAULT '0',
  `dex` int(10) NOT NULL DEFAULT '0',
  `int` int(10) NOT NULL DEFAULT '0',
  `luk` int(10) NOT NULL DEFAULT '0',
  `hp` int(10) NOT NULL DEFAULT '0',
  `mp` int(10) NOT NULL DEFAULT '0',
  `solid` int(10) NOT NULL DEFAULT '0',
  `max_ability` int(10) NOT NULL DEFAULT '0',
  `ability` int(10) NOT NULL DEFAULT '0',
  `lv_cost` int(10) NOT NULL DEFAULT '0',
  `trade` int(2) NOT NULL DEFAULT '1',
  `sell` int(2) NOT NULL DEFAULT '1',
  `use` int(2) NOT NULL DEFAULT '1',
  `character` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `ecollection` int(11) NOT NULL DEFAULT '0',
  `emining` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=1988 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_equipment`
--

LOCK TABLES `storage_equipment` WRITE;
/*!40000 ALTER TABLE `storage_equipment` DISABLE KEYS */;
INSERT INTO `storage_equipment` VALUES (1696,59,38,5,5,0,5,0,0,0,212,212,0,1,1,1,'',0,0),(1730,62,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1826,59,35,4,5,0,1,0,0,0,122,122,0,1,1,1,'',0,0),(1699,59,2,3,5,0,5,0,0,0,218,218,0,1,1,1,'',0,0),(1753,59,1,4,3,0,2,0,0,0,161,161,0,1,1,1,'',0,0),(1915,59,40,5,1,0,4,0,0,0,177,177,0,1,1,1,'',0,0),(1811,61,0,16,24,13,16,0,0,0,365,365,0,1,1,1,'',0,0),(1703,60,99,8,10,7,0,0,0,0,101,101,0,1,1,1,'',0,0),(1929,60,30,4,4,8,4,0,0,0,202,202,0,1,1,1,'',0,0),(1706,61,0,26,23,20,19,0,0,0,405,405,0,1,1,1,'',0,0),(1709,59,27,5,3,0,4,0,0,0,288,288,0,1,1,1,'',0,0),(1823,62,276,20,19,11,9,0,0,0,302,302,0,1,1,1,'',0,0),(1748,61,0,28,8,12,14,0,0,0,360,360,0,1,1,1,'',0,0),(1712,61,0,25,6,28,18,0,0,0,237,237,0,1,1,1,'',0,0),(1922,59,49,4,4,0,5,0,0,0,286,286,0,1,1,1,'',0,0),(1760,66,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1824,63,868,47,47,37,8,0,0,0,848,848,0,1,1,1,'',0,0),(1726,63,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1921,60,25,7,3,0,2,0,0,0,134,134,0,1,1,1,'',0,0),(1927,63,133,44,22,34,15,0,0,0,994,994,0,1,1,1,'',0,0),(1793,62,113,20,0,8,4,0,0,0,406,406,0,1,1,1,'',0,0),(1745,60,12,10,8,5,2,0,0,0,153,153,0,1,1,1,'',0,0),(1801,61,0,28,4,16,9,0,0,0,250,250,0,1,1,1,'',0,0),(1765,61,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0),(1725,60,68,9,10,2,3,0,0,0,125,125,0,1,1,1,'',0,0),(1789,60,92,10,2,9,7,0,0,0,205,205,0,1,1,1,'',0,0),(1788,59,42,3,2,0,4,0,0,0,173,173,0,1,1,1,'',0,0),(1890,60,47,10,10,10,6,0,0,0,294,294,0,1,1,1,'',0,0),(1926,60,85,2,2,4,1,0,0,0,281,281,0,1,1,1,'',0,0),(1901,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1770,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1771,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1772,66,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1918,59,4,0,0,0,2,0,0,0,175,175,0,1,1,1,'',0,0),(1917,60,77,1,2,7,1,0,0,0,137,137,0,1,1,1,'',0,0),(1779,59,23,4,2,0,2,0,0,0,276,276,0,1,1,1,'',0,0),(1930,62,259,20,14,11,3,0,0,0,655,655,0,1,1,1,'',0,0),(1916,59,23,5,3,0,0,0,0,0,236,236,0,1,1,1,'',0,0),(1815,59,34,3,4,0,2,0,0,0,196,196,0,1,1,1,'',0,0),(1818,66,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1817,66,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1820,60,99,10,10,9,0,0,0,0,171,171,0,1,1,1,'',0,0),(1881,60,48,9,9,6,6,0,0,0,120,120,0,1,1,1,'',0,0),(1920,63,142,13,1,20,7,0,0,0,898,898,0,1,1,1,'',0,0),(1844,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1845,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1846,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1847,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1848,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1849,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1928,59,20,5,4,0,3,0,0,0,168,168,0,1,1,1,'',0,0),(1900,60,88,1,10,0,1,0,0,0,230,230,0,1,1,1,'',0,0),(1899,62,53,12,11,10,0,0,0,0,573,573,0,1,1,1,'',0,0),(1888,62,31,8,10,10,3,0,0,0,648,648,0,1,1,1,'',0,0),(1898,62,106,4,10,5,7,0,0,0,541,541,0,1,1,1,'',0,0),(1903,63,930,48,37,25,21,0,0,0,727,727,0,1,1,1,'',0,0),(1896,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1923,60,3,10,8,0,5,0,0,0,119,119,0,1,1,1,'',0,0),(1871,62,155,15,12,18,7,0,0,0,628,628,0,1,1,1,'',0,0),(1919,59,38,3,4,0,1,0,0,0,149,149,0,1,1,1,'',0,0),(1924,59,37,3,2,0,2,0,0,0,252,252,0,1,1,1,'',0,0),(1886,63,267,6,19,22,5,0,0,0,930,930,0,1,1,1,'',0,0),(1925,59,12,0,4,0,0,0,0,0,138,138,0,1,1,1,'',0,0),(1884,62,193,17,2,1,2,0,0,0,538,538,0,1,1,1,'',0,0),(1879,63,82,33,6,17,20,0,0,0,908,908,0,1,1,1,'',0,0),(1880,63,408,12,12,19,1,0,0,0,808,808,0,1,1,1,'',0,0),(1931,72,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0),(1932,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1933,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1934,63,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1935,63,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1936,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1937,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1938,74,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0),(1939,72,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0),(1941,72,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0),(1942,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1943,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1944,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1945,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1946,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1947,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1948,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1949,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1950,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1951,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1952,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1953,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1954,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1955,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1956,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1957,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1958,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1959,62,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1960,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1961,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1962,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1963,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1964,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1965,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1966,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1967,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1968,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1969,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1970,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1971,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1972,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1973,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1974,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1975,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1976,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1977,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1978,59,0,0,0,0,0,0,0,0,100,100,0,1,1,1,'',0,0),(1979,72,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0),(1980,62,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',8,10),(1981,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1982,66,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1983,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1984,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0),(1985,65,0,0,0,0,0,0,0,0,400,400,0,1,1,1,'',0,0),(1986,63,0,0,0,0,0,0,0,0,200,200,0,1,1,1,'',0,0),(1987,64,0,0,0,0,0,0,0,0,350,350,0,1,1,1,'',0,0);
/*!40000 ALTER TABLE `storage_equipment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_item`
--

DROP TABLE IF EXISTS `storage_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_item` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `name` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `type` int(3) NOT NULL DEFAULT '0',
  `equip_type` int(3) NOT NULL DEFAULT '0',
  `icon` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `description` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `price` int(10) NOT NULL DEFAULT '0',
  `str` int(10) NOT NULL DEFAULT '0',
  `dex` int(10) NOT NULL DEFAULT '0',
  `int` int(10) NOT NULL DEFAULT '0',
  `luk` int(10) NOT NULL DEFAULT '0',
  `hp` int(10) NOT NULL DEFAULT '0',
  `mp` int(10) NOT NULL DEFAULT '0',
  `solid` int(10) NOT NULL DEFAULT '0',
  `max_ability` int(10) NOT NULL DEFAULT '0',
  `ability` int(10) NOT NULL DEFAULT '0',
  `lv_cost` int(10) NOT NULL DEFAULT '0',
  `rank` int(10) NOT NULL DEFAULT '0',
  `trade` int(2) NOT NULL DEFAULT '1',
  `sell` int(2) NOT NULL DEFAULT '1',
  `use` int(2) NOT NULL DEFAULT '1',
  `method_name` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `method_arg` int(10) NOT NULL,
  `animation_id` int(10) NOT NULL,
  `isown` int(2) DEFAULT '0',
  `ecollection` int(11) NOT NULL DEFAULT '0',
  `emining` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=77 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_item`
--

LOCK TABLES `storage_item` WRITE;
/*!40000 ALTER TABLE `storage_item` DISABLE KEYS */;
INSERT INTO `storage_item` VALUES (58,'다람쥐의 도토리',3,0,'042-Item11.png','다람쥐가 숨기려던 도토리다.',5,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0,0,0,0),(59,'다람쥐 귀걸이',0,0,'ear.png','다람쥐가 실수로 먹은 귀걸이. 그래서 다람쥐 귀걸이라는 이름이 붙여졌다.',50,3,1,0,0,0,0,10,100,100,0,1,1,1,1,'',0,0,0,0,0),(60,'들개가죽 신발',0,8,'shoes.png','들개의 가죽으로 만들어진 신발이다.',100,2,2,0,2,0,0,50,100,100,0,2,1,1,1,'',0,0,0,0,0),(61,'들개 이름표',0,2,'neck.png','들개가 사실 유기견이었던 것 같다.',200,5,5,0,5,0,0,70,100,100,0,3,1,1,1,'',0,0,0,0,0),(62,'호랑이 갑옷',0,4,'armor.png','호랑이 형님의 강인함이 담긴 갑옷이다.',300,10,10,3,0,0,0,200,200,200,5,4,1,1,1,'',0,0,0,0,0),(63,'호랑이 투구',0,1,'helmet.png','호랑의 형님의 강인함이 담긴 투구이다. 진정한 용사에게만 주어진다는 전설도 있다고 한다.',500,15,15,5,5,0,0,500,200,200,5,5,1,1,1,'',0,0,0,2,3),(64,'어둠의 힘이 깃든 검 + 5',0,3,'weapon.png','어둠의 힘이 깃들어 더욱 강력한 힘을 발휘하는 검이다. 특별한 경우에만 얻을 수 있다고 전해진다.',2000,110,10,0,30,0,0,500,350,350,1,5,1,1,1,'',0,0,0,10,1),(65,'어둠의 방패 + 5',0,5,'shield.png','어둠의 힘이 깃들어 더욱 강력한 힘을 발휘하는 방패다. 특별한 경우에만 얻을 수 있다고 전해진다.',2000,0,100,0,0,1000,0,500,400,400,1,5,1,1,1,'',0,0,0,0,0),(66,'어둠의 벨트',0,9,'belt.png','어둠의 힘이 깃든 벨트지만 이상하게 흔한 벨트다.',300,20,10,0,10,200,0,500,200,200,0,3,1,1,1,'',0,0,0,0,0),(67,'독버섯 잔해',3,0,'043-Item12.png','독버섯의 잔해다.',130,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0,0,0,0),(68,'호랑이의 정기',3,0,'036-Item05.png','호랑이의 힘이 담겨있는 정기이다.',1000,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0,0,0,0),(69,'회복포션 Lv.1',1,0,'021-Potion01.png','사용시 즉시 200의 HP를 회복합니다.',1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'RecoveryHpValue',200,15,0,0,0),(70,'기술의 서 - 파이어',1,0,'book.png','파이어 기술을 배울 수 있는 기술의 서.',5000,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'LearnCharacterSkill',1,0,0,0,0),(71,'기술의 서 - 프론트 어택',1,0,'book.png','프론트 어택 기술을 배울 수 있는 기술의 서.',0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'LearnCharacterSkill',2,0,0,0,0),(72,'SIlver Robes',0,4,'013-Body01','',0,10,5,5,197,1000,0,0,0,0,0,4,1,1,1,'',0,0,0,0,0),(73,'Infinite',1,0,'book.png','',0,0,0,0,0,0,0,0,0,0,0,5,1,1,1,'LearnCharacterSkill',6,2,0,0,0),(74,'Arc',0,3,'weapon.png','세계선 Arc의 복제버전, 스킬은 사용할 수 없지만, 그 막강함은 유지하고 있다..',0,200,50,100,99,1000,500,0,0,0,0,5,1,1,1,'',0,0,0,0,0),(75,'벼 씨앗',3,0,'042-Item11.png','벼를 키울 수 있는 씨앗이다. 도토리처럼 생긴건 기분탓이다.',0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0,0,0,0),(76,'벼',3,0,'042-Item11.png','벼다. 도토리처럼 생긴건 기분탓입다.',0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,'',0,0,0,5,1);
/*!40000 ALTER TABLE `storage_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_job`
--

DROP TABLE IF EXISTS `storage_job`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_job` (
  `no` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `info` varchar(100) NOT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_job`
--

LOCK TABLES `storage_job` WRITE;
/*!40000 ALTER TABLE `storage_job` DISABLE KEYS */;
INSERT INTO `storage_job` VALUES (1,'마법사','원소를 다룬다.');
/*!40000 ALTER TABLE `storage_job` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_job_skill`
--

DROP TABLE IF EXISTS `storage_job_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_job_skill` (
  `no` int(11) NOT NULL AUTO_INCREMENT,
  `jobno` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `skillno` int(11) NOT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_job_skill`
--

LOCK TABLES `storage_job_skill` WRITE;
/*!40000 ALTER TABLE `storage_job_skill` DISABLE KEYS */;
INSERT INTO `storage_job_skill` VALUES (1,1,2,2),(2,1,5,3);
/*!40000 ALTER TABLE `storage_job_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_recipe`
--

DROP TABLE IF EXISTS `storage_recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_recipe` (
  `no` int(10) NOT NULL DEFAULT '0',
  `name` char(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `element` char(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `result` char(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_recipe`
--

LOCK TABLES `storage_recipe` WRITE;
/*!40000 ALTER TABLE `storage_recipe` DISABLE KEYS */;
INSERT INTO `storage_recipe` VALUES (1,'정기','58:2,59:1','68:1');
/*!40000 ALTER TABLE `storage_recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_skill`
--

DROP TABLE IF EXISTS `storage_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_skill` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `name` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `description` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `icon` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `function` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `max_level` int(10) NOT NULL DEFAULT '1',
  `power` int(10) NOT NULL DEFAULT '0',
  `power_factor` int(10) NOT NULL DEFAULT '1',
  `level_power` int(10) NOT NULL DEFAULT '0',
  `cost` int(10) NOT NULL DEFAULT '0',
  `range_type` int(10) NOT NULL DEFAULT '0',
  `range` int(10) NOT NULL DEFAULT '1',
  `count` int(10) NOT NULL DEFAULT '1',
  `delay` int(10) NOT NULL DEFAULT '0',
  `wait_time` int(10) NOT NULL DEFAULT '0',
  `use_animation` int(10) NOT NULL DEFAULT '0',
  `target_animation` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_skill`
--

LOCK TABLES `storage_skill` WRITE;
/*!40000 ALTER TABLE `storage_skill` DISABLE KEYS */;
INSERT INTO `storage_skill` VALUES (1,'파이어어택','공격함','044-Skill01.png','attack',1,200,1,100,30,0,3,3,2,60,2,27),(2,'프론트 어택','공격함','044-Skill01.png','attack',1,200,2,100,30,1,1,1,0,10,0,10),(5,'절대 영도','모든 분자의 이동이 멈춘다.','044-Skill01.png','attack',1,100,1,0,0,4,5,10,1,0,2,31),(6,'Finger Snap','','044-Skill01.png','attack',1,1000,1,0,20,4,3,1,0,60,2,999);
/*!40000 ALTER TABLE `storage_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storageitem_list`
--

DROP TABLE IF EXISTS `storageitem_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storageitem_list` (
  `no` int(10) NOT NULL DEFAULT '0',
  `storageno` int(10) DEFAULT NULL,
  `itemno` int(10) DEFAULT NULL,
  `itemnum` int(10) DEFAULT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storageitem_list`
--

LOCK TABLES `storageitem_list` WRITE;
/*!40000 ALTER TABLE `storageitem_list` DISABLE KEYS */;
/*!40000 ALTER TABLE `storageitem_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `store`
--

DROP TABLE IF EXISTS `store`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `store` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `name` char(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `store`
--

LOCK TABLES `store` WRITE;
/*!40000 ALTER TABLE `store` DISABLE KEYS */;
INSERT INTO `store` VALUES (1,'테스트'),(2,'어둠의 상점');
/*!40000 ALTER TABLE `store` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `store_item`
--

DROP TABLE IF EXISTS `store_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `store_item` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `store_no` int(10) DEFAULT NULL,
  `item_no` int(10) DEFAULT NULL,
  `price` int(10) DEFAULT '0',
  `number` int(10) DEFAULT '0',
  `discount` int(10) DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=24 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `store_item`
--

LOCK TABLES `store_item` WRITE;
/*!40000 ALTER TABLE `store_item` DISABLE KEYS */;
INSERT INTO `store_item` VALUES (11,1,61,300,-1,0),(13,1,59,100,-1,0),(8,2,64,0,1,0),(9,2,65,10000,1,0),(10,2,66,1000,50,0),(14,1,60,250,-1,0),(15,1,62,400,10,0),(16,1,63,500,10,0),(17,1,69,2,-1,0),(19,1,70,1,-1,0),(20,1,71,1,-1,0),(21,1,72,0,-1,0),(22,1,73,0,-1,0),(23,1,74,0,-1,0);
/*!40000 ALTER TABLE `store_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_character`
--

DROP TABLE IF EXISTS `user_character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_character` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `user_no` int(10) NOT NULL,
  `name` char(255) COLLATE utf8_unicode_ci NOT NULL,
  `image` char(255) COLLATE utf8_unicode_ci NOT NULL DEFAULT '001-Fighter01.png',
  `exp` int(10) NOT NULL,
  `gold` int(10) NOT NULL DEFAULT '0',
  `level` int(10) NOT NULL DEFAULT '1',
  `guild` int(10) NOT NULL,
  `job` int(10) NOT NULL DEFAULT '0',
  `maxhp` int(10) NOT NULL DEFAULT '100',
  `maxmp` int(10) NOT NULL DEFAULT '100',
  `hp` int(10) NOT NULL DEFAULT '100',
  `mp` int(10) NOT NULL DEFAULT '100',
  `str` int(10) NOT NULL DEFAULT '5',
  `dex` int(10) NOT NULL DEFAULT '5',
  `int` int(10) NOT NULL DEFAULT '5',
  `luk` int(10) NOT NULL DEFAULT '5',
  `point` int(10) NOT NULL DEFAULT '0',
  `map_id` int(10) NOT NULL DEFAULT '1',
  `map_x` int(10) NOT NULL DEFAULT '0',
  `map_y` int(10) NOT NULL DEFAULT '0',
  `direction` int(10) NOT NULL DEFAULT '2',
  `move_speed` int(10) NOT NULL DEFAULT '4',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=61 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_character`
--

LOCK TABLES `user_character` WRITE;
/*!40000 ALTER TABLE `user_character` DISABLE KEYS */;
INSERT INTO `user_character` VALUES (33,201,'admin','008-Fighter08.png',50,4766196,2,0,1,100,100,61,100,34,14,14,12,55,1,11,10,6,4),(40,205,'ㅇ','003-Fighter03.png',0,0,1,0,0,100,100,775,100,5,5,5,5,0,1,22,8,6,4),(47,212,'test1','001-Fighter01.png',10,0,1,0,0,100,100,1100,100,5,5,5,5,0,1,11,9,2,4),(48,213,'test2','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,1,11,9,6,4),(49,214,'test3','001-Fighter01.png',240,58,7,0,0,100,100,2250,100,5,5,5,5,0,1,11,9,6,4),(51,215,'test4','002-Fighter02.png',0,0,1,0,0,100,100,1100,100,5,5,5,5,0,1,15,8,4,4),(52,216,'test5','001-Fighter01.png',0,100000000,1,0,0,100,100,100,100,5,5,5,5,0,1,1,7,4,4),(53,217,'test6','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,1,0,0,2,4),(54,218,'test7','001-Fighter01.png',0,0,1,0,0,100,100,85,100,5,5,5,5,0,1,12,11,2,4),(55,219,'test8','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,1,12,5,6,4),(56,220,'1234','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,2,4,9,2,4),(57,221,'test11','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,2,4,8,6,4),(58,222,'test12','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,2,4,8,6,4),(59,223,'test20','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,1,0,0,2,4),(60,224,'test21','001-Fighter01.png',0,0,1,0,0,100,100,100,100,5,5,5,5,0,1,7,7,6,4);
/*!40000 ALTER TABLE `user_character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_equipment`
--

DROP TABLE IF EXISTS `user_equipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_equipment` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `char_no` int(10) NOT NULL,
  `item_no` int(10) NOT NULL,
  `seed` int(10) DEFAULT '-1',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=569 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_equipment`
--

LOCK TABLES `user_equipment` WRITE;
/*!40000 ALTER TABLE `user_equipment` DISABLE KEYS */;
INSERT INTO `user_equipment` VALUES (566,33,1973,-1);
/*!40000 ALTER TABLE `user_equipment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_information`
--

DROP TABLE IF EXISTS `user_information`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_information` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `id` char(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `pw` char(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `mail` char(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `pass_question` char(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `pass_answer` char(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `online` int(5) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=225 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_information`
--

LOCK TABLES `user_information` WRITE;
/*!40000 ALTER TABLE `user_information` DISABLE KEYS */;
INSERT INTO `user_information` VALUES (201,'admin','admin','admin@naver.com','가장 좋아하는 물건은?','admin',0),(205,'d','d','d@naver.com','가장 좋아하는 물건은?','ㅇ',0),(212,'test1','test1','test1@naver.com','?? ?? ????','????1',1),(213,'test2','test2','test2@nate.com','?? ?? ????','????2',0),(214,'test3','1234','1234@naver.com','???? ????','1234',0),(215,'test4','1234','1234@hanmail.net','?? ?? ????','1234',0),(216,'test5','1234','1234@hanmail.net','?? ?? ????','1234',0),(217,'test6','1234','1234@nate.com','?? ?? ????','1234',1),(218,'test7','1234','1234@nate.com','?? ?? ????','1234',0),(219,'test8','1234','1234@nate.com','?? ???? ????','1234',0),(220,'test10','1234','1234@naver.com','???? ????','1234',0),(221,'test11','1234','1234@nate.com','?? ?? ????','1234',0),(222,'test12','1234','1234@naver.com','?? ???? ????','1234',0),(223,'test20','1234','test20@naver.com','나의 출생 고향은?','1234',1),(224,'test21','1234','test21@nate.com','나의 출생 고향은?','1234',0);
/*!40000 ALTER TABLE `user_information` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_inventory`
--

DROP TABLE IF EXISTS `user_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_inventory` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `char_no` int(10) NOT NULL,
  `item_no` int(10) NOT NULL,
  `item_type` int(10) NOT NULL,
  `number` int(10) NOT NULL DEFAULT '0',
  `seed` int(10) DEFAULT '-1',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=2690 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_inventory`
--

LOCK TABLES `user_inventory` WRITE;
/*!40000 ALTER TABLE `user_inventory` DISABLE KEYS */;
INSERT INTO `user_inventory` VALUES (2689,33,76,3,1,0),(2688,33,75,3,1,0),(2687,33,1987,0,0,0),(2686,33,1986,0,0,0),(2685,33,1985,0,0,0);
/*!40000 ALTER TABLE `user_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_skill`
--

DROP TABLE IF EXISTS `user_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_skill` (
  `no` int(10) NOT NULL AUTO_INCREMENT,
  `char_no` int(10) NOT NULL,
  `skill_no` int(10) NOT NULL,
  `level` int(10) NOT NULL DEFAULT '1',
  `wait_time` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`no`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_skill`
--

LOCK TABLES `user_skill` WRITE;
/*!40000 ALTER TABLE `user_skill` DISABLE KEYS */;
INSERT INTO `user_skill` VALUES (4,49,6,1,0),(5,49,2,1,0),(6,49,1,1,0),(7,33,2,1,0);
/*!40000 ALTER TABLE `user_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'net_rmxp_project'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-11-19  0:54:47
